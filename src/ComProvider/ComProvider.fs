namespace ComProvider

open System
open System.IO
open System.Reflection
open Microsoft.FSharp.Core.CompilerServices
open Microsoft.FSharp.Quotations
open Microsoft.Win32
open Samples.FSharp.ProvidedTypes
open System.Collections.Generic
open ComProvider.TypeLibInfo
open ComProvider.TypeLibImport

[<TypeProvider>]
type ComProvider(cfg:TypeProviderConfig) as this =
    inherit TypeProviderForNamespaces()
    
    let asm = Assembly.GetExecutingAssembly()
    
    let types =
        [ for name, versions in loadTypeLibs() |> Seq.groupBy (fun l -> l.Name) do
            let nameTy = ProvidedTypeDefinition(asm, "TypeLib", name, None)
            yield nameTy
            for version in versions do
               let versionTy = ProvidedTypeDefinition(TypeContainer.TypeToBeDecided, version.Version.String, None)
               nameTy.AddMember(versionTy)
               versionTy.IsErased <- false
               versionTy.AddAssemblyTypesAsNestedTypesDelayed <| fun _ -> 
                   let assembly = TypeLibImport.importFromPath version.Path cfg.TemporaryFolder :> Assembly
                   ProvidedAssembly.RegisterGenerated(Path.Combine(cfg.TemporaryFolder, assembly.GetName().Name + ".dll")) ]
    
    do  this.AddNamespace("TypeLib", types)
        this.RegisterProbingFolder(cfg.TemporaryFolder)

[<TypeProviderAssembly>]
()
