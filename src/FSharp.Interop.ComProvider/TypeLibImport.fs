module private FSharp.Interop.ComProvider.TypeLibImport

open System
open System.IO
open System.Reflection
open System.Runtime.InteropServices
open System.Runtime.InteropServices.ComTypes
open Utility

[<DllImport("oleaut32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)>]
extern void private LoadTypeLib(string filename, ITypeLib& typelib);

let private converter = TypeLibConverter()

let rec private convertToAsm typeLib asmDir =
    let sink = { new ITypeLibImporterNotifySink with
        member this.ReportEvent(eventKind, eventCode, eventMsg) = ()
        member this.ResolveRef(typeLib) = convertToAsm typeLib asmDir :> Assembly}
    let flags = TypeLibImporterFlags.None
    let asmFile = Guid.NewGuid().ToString() + ".dll"
    let asmPath = Path.Combine(asmDir, asmFile)
    converter.ConvertTypeLibToAssembly(typeLib, asmPath, flags, sink, null, null, "", null)

let importTypeLib typeLibPath asmDir =
    let mutable typeLib : ITypeLib = null
    LoadTypeLib(typeLibPath, &typeLib)
    let asm = convertToAsm typeLib asmDir
    asm.Save(asm.GetName().Name + ".dll")
    asm
