namespace FSharp.Interop.ComProvider

open System
open System.IO
open System.Reflection
open Microsoft.FSharp.Core.CompilerServices
open FSharp.Interop.ComProvider.ProvidedTypes
open TypeLibInfo
open TypeLibImport

[<TypeProvider>]
type ComProvider(cfg:TypeProviderConfig) as this =
    inherit TypeProviderForNamespaces()
    
    let asm = Assembly.GetExecutingAssembly()
    
    let preferredPlatform =
        if cfg.IsHostedExecution && Environment.Is64BitProcess then "win64"
        else "win32"

    let types =
        [ for name, versions in loadTypeLibs preferredPlatform |> Seq.groupBy (fun l -> l.Name) do
            let nameTy = ProvidedTypeDefinition(asm, "TypeLib", name, None)
            yield nameTy
            for version in versions do
               let versionTy = ProvidedTypeDefinition(TypeContainer.TypeToBeDecided, version.Version.String, None)
               nameTy.AddMember(versionTy)
               versionTy.IsErased <- false
               versionTy.AddAssemblyTypesAsNestedTypesDelayed <| fun _ -> 
                   let assembly = importTypeLib version.Path cfg.TemporaryFolder :> Assembly
                   ProvidedAssembly.RegisterGenerated(Path.Combine(cfg.TemporaryFolder, assembly.GetName().Name + ".dll")) ]
    
    do  this.AddNamespace("TypeLib", types)
        this.RegisterProbingFolder(cfg.TemporaryFolder)

[<TypeProviderAssembly>]
()
