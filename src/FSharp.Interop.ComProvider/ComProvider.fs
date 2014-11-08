namespace FSharp.Interop.ComProvider

open System
open System.IO
open System.Reflection
open Microsoft.FSharp.Core.CompilerServices
open FSharp.Interop.ComProvider.ProvidedTypes
open TypeLibInfo
open TypeLibImport
open TypeLibDoc

[<TypeProvider>]
type ComProvider(cfg:TypeProviderConfig) as this =
    inherit TypeProviderForNamespaces()
    
    let asm = Assembly.GetExecutingAssembly()
    
    // The TypeLib registry key allows specifying separate type libraries for each CPU
    // platform. However, there is no way to know what the target platform will be when
    // compiling a project, since this information is not available to type providers.
    // Furthermore, the target platform isn't actually known until runtime if Any CPU
    // is selected. I think it would be unusual for the metadata to differ between type
    // libraries for different platforms, but for consistency we'll always prefer the
    // 32-bit type library when compiling. When running in-process, such as with FSI,
    // we'll prefer the platform of the host process.
    let preferredPlatform =
        if cfg.IsHostedExecution && Environment.Is64BitProcess then "win64"
        else "win32"

    // We use nested types as opposed to namespaces for the following reasons:
    // 1. No way with ProvidedTypes API to have sub-namespaces generated on demand.
    // 2. Namespace components cannot contain dots, which are common both in the
    // type library name itself and of course the major.minor version number.
    let types =
        [ for name, versions in loadTypeLibs preferredPlatform |> Seq.groupBy (fun l -> l.Name) do
            let nameTy = ProvidedTypeDefinition(asm, "COM", name, None)
            yield nameTy
            for version in versions do
               let lazyTypeLib = lazy loadTypeLib version.Path
               let lazyTypeLibDoc = lazy getTypeLibDoc lazyTypeLib.Value
               let versionTy = ProvidedTypeDefinition(TypeContainer.TypeToBeDecided, version.Version.String, None)
               nameTy.AddMember(versionTy)
               versionTy.IsErased <- false
               versionTy.AddXmlDocDelayed <| fun _ ->
                    let name, docString, _ = lazyTypeLibDoc.Value
                    sprintf "%s (%s)" docString name
               versionTy.AddMembersDelayed <| fun _ ->
                   let _, _, typeDocs = lazyTypeLibDoc.Value
                   let asm = importTypeLib lazyTypeLib.Value cfg.TemporaryFolder
                   let savedAsm = ProvidedAssembly.RegisterGenerated(Path.Combine(cfg.TemporaryFolder, asm.GetName().Name + ".dll"))
                   let annotatedAsm = savedAsm |> annotateAssembly typeDocs
                   annotatedAsm.GetTypes() |> Array.toList ]

    do  this.AddNamespace("COM", types)
        this.RegisterProbingFolder(cfg.TemporaryFolder)

[<TypeProviderAssembly>]
()
