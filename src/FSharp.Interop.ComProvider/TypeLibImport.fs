module private FSharp.Interop.ComProvider.TypeLibImport

open System
open System.IO
open System.Reflection
open System.Runtime.InteropServices
open System.Runtime.InteropServices.ComTypes

[<DllImport("oleaut32.dll", CharSet = CharSet.Unicode, ExactSpelling = true)>]
extern void private LoadTypeLib(string filename, ITypeLib& typelib);

let rec private convertToAsm typeLib asmDir =
    let converter = TypeLibConverter()
    let sink = { new ITypeLibImporterNotifySink with
        member __.ReportEvent(eventKind, eventCode, eventMsg) = ()
        member __.ResolveRef(typeLib) = convertToAsm typeLib asmDir :> Assembly}
    let flags = TypeLibImporterFlags.None
    let asmFile = Guid.NewGuid().ToString() + ".dll"
    let asmPath = Path.Combine(asmDir, asmFile)
    converter.ConvertTypeLibToAssembly(typeLib, asmPath, flags, sink, null, null, null, null)

let loadTypeLib path =
    let mutable typeLib : ITypeLib = null
    LoadTypeLib(path, &typeLib)
    if typeLib = null then
        failwith ("Error loading type library. Please check that the " +
                  "component is correctly installed and registered.")
    else typeLib

let importTypeLib (typeLib:ITypeLib) asmDir =
    let asm = convertToAsm typeLib asmDir
    asm.Save(asm.GetName().Name + ".dll")
    asm :> Assembly
