namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("ComProvider")>]
[<assembly: AssemblyProductAttribute("ComProvider")>]
[<assembly: AssemblyDescriptionAttribute("F# type provider for COM interop")>]
[<assembly: AssemblyVersionAttribute("1.0.0")>]
[<assembly: AssemblyFileVersionAttribute("1.0.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "1.0.0"
