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
    
    do  this.RegisterProbingFolder(cfg.TemporaryFolder)
        for lib in loadTypeLibs() do
           let ns = sprintf "TypeLib.%s" lib.Name
           let ty = ProvidedTypeDefinition(asm, ns, lib.Version.String, None)
           ty.IsErased <- false
           ty.AddAssemblyTypesAsNestedTypesDelayed <| fun _ -> 
               let assembly = TypeLibImport.importFromPath lib.Path cfg.TemporaryFolder :> Assembly
               ProvidedAssembly.RegisterGenerated(Path.Combine(cfg.TemporaryFolder, assembly.GetName().Name + ".dll"))
           this.AddNamespace(ns, [ty])

[<TypeProviderAssembly>]
()
