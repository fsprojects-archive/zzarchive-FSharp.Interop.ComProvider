module private FSharp.Interop.ComProvider.TypeLibImport

open System.IO
open System.Reflection
open System.Runtime.InteropServices
open System.Runtime.InteropServices.ComTypes
open TypeLibDoc
open System

[<DllImport("oleaut32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)>]
extern void private LoadTypeLib(string filename, ITypeLib& typelib);

let loadTypeLib path =
    let mutable typeLib : ITypeLib = null
    LoadTypeLib(path, &typeLib)
    if typeLib = null then
        failwith ("Error loading type library. Please check that the " +
                  "component is correctly installed and registered.")
    typeLib

let rec importTypeLib path asmDir =
    let assemblies = ResizeArray<Assembly>()
    let rec convertToAsm (typeLib:ITypeLib) =
        let converter = TypeLibConverter()
        let libName = Marshal.GetTypeLibName(typeLib)
        let asmFile = libName + ".dll"
        let asmPath = Path.Combine(asmDir, asmFile)
        let flags = TypeLibImporterFlags.None
        let sink = { new ITypeLibImporterNotifySink with
            member __.ReportEvent(eventKind, eventCode, eventMsg) = ()
            member __.ResolveRef(typeLib) = convertToAsm (typeLib :?> ITypeLib) }
        let asm = converter.ConvertTypeLibToAssembly(typeLib, asmPath, flags, sink, null, null, libName, null)
        asm.Save(asmFile)
        let typeDocs = getTypeLibDoc typeLib
        Assembly.LoadFrom(asmPath)
        |> annotateAssembly typeDocs
        |> assemblies.Add
        asm :> Assembly
    let typeLib = loadTypeLib path
    convertToAsm typeLib |> ignore
    Marshal.FinalReleaseComObject(typeLib) |> ignore
    assemblies |> Seq.toList
