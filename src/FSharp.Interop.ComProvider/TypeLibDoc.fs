module private FSharp.Interop.ComProvider.TypeLibDoc

open System
open System.Runtime.InteropServices
open System.Runtime.InteropServices.ComTypes
open System.Reflection
open System.Collections.Generic
open Microsoft.FSharp.Core.CompilerServices
open Delegators

let getStruct<'t when 't : struct> ptr freePtr =
    let str = Marshal.PtrToStructure(ptr, typeof<'t>) :?> 't
    freePtr ptr
    str

let getTypeLibDoc (typeLib:ITypeLib) =
    [ for typeIndex = 0 to typeLib.GetTypeInfoCount() - 1 do
        let typeInfo = typeLib.GetTypeInfo(typeIndex)
        let typeAttr = getStruct<TYPEATTR> (typeInfo.GetTypeAttr()) typeInfo.ReleaseTypeAttr
        let typeName, typeDoc, _, _ = typeInfo.GetDocumentation(-1)
        let memberDocs =
            [ for funcIndex = 0 to int typeAttr.cFuncs - 1 do
                let funcDesc = getStruct<FUNCDESC> (typeInfo.GetFuncDesc(funcIndex)) typeInfo.ReleaseFuncDesc
                let funcName, funcDoc, _, _ = typeInfo.GetDocumentation(funcDesc.memid)
                yield funcName, funcDoc ]
        yield typeName, (typeDoc, Map.ofSeq memberDocs) ]
    |> Map.ofSeq

let annotateAssembly typeDocs (asm:Assembly) =
    let toList (items:seq<'t>) = ResizeArray<'t> items :> IList<'t>

    let attrCons = typeof<TypeProviderXmlDocAttribute>.GetConstructor [| typeof<string> |]
    let attrData docString =
        { new CustomAttributeData() with
            override __.Constructor = attrCons
            override __.ConstructorArguments = [ CustomAttributeTypedArgument docString ] |> toList }
    let addAttr docString (memb:MemberInfo) =
        if String.IsNullOrWhiteSpace docString then []
        else [attrData docString]
        |> Seq.append (memb.GetCustomAttributesData())
        |> toList

    let typeDoc (ty:Type) =
        typeDocs
        |> Map.tryFind ty.Name
        |> Option.map fst

    let memberDoc (memb:MemberInfo) =
        let ty = memb.DeclaringType
        ty.GetInterfaces()
        |> Seq.append [ty]
        |> Seq.choose (fun ty -> typeDocs |> Map.tryFind ty.Name)
        |> Seq.choose (fun (_, membs) -> membs |> Map.tryFind memb.Name)
        |> Seq.tryFind (not << String.IsNullOrEmpty)

    let annotate getDoc addAnnotation (memb:#MemberInfo) =
        let doc =
            match getDoc memb with
            | Some doc -> doc
            | None -> ""
        addAnnotation (addAttr doc memb) memb

    let annotateMethod = annotate memberDoc <| fun data meth ->
        { new MethodInfoDelegator(meth) with
            override __.GetCustomAttributesData() = data } :> MethodInfo

    let annotateProperty = annotate memberDoc <| fun data prop ->
        { new PropertyInfoDelegator(prop) with
            override __.GetCustomAttributesData() = data } :> PropertyInfo

    let annotateType = annotate typeDoc <| fun attr ty ->
        { new TypeDelegator(ty) with
            override __.GetCustomAttributesData() = attr
            override __.GetMethods(flags) = ty.GetMethods(flags) |> Array.map annotateMethod 
            override __.GetProperties(flags) = ty.GetProperties(flags) |> Array.map annotateProperty
            override __.GetMembers(flags) = ty.GetMembers(flags) } :> Type

    { new AssemblyDelegator(asm) with
        override __.GetTypes() = asm.GetTypes() |> Array.map annotateType } :> Assembly
