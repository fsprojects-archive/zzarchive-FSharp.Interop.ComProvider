(*** hide ***)
// This block of code is omitted in the generated HTML documentation. Use
// it to define helpers that you do not want to show in the documentation.
#I "../../bin"

open System

(**
COM Type Provider for F#
========================

The COM Type Provider provides a new way to do COM interop from F#. It allows
you to access COM components directly from F# projects and scripts without
adding any references other than the type provider itself.

<p>
    <img src="img/intellisense-sample.png" class="center"
         alt="Visual Studio Intellisense Sample" />
</p>

One advantage of this method is that you can author and deploy F# scripts
without having to pre-generate the interop assemblies. Another advantage is
that you can easily explore all the COM components installed on your machine
via intellisense.

<div class="well well-small center" id="nuget">
    The Com Type Provider can be
    <a href="https://www.nuget.org/packages/FSharp.Interop.ComProvider/">installed from NuGet</a>:
    <pre>PM> Install-Package FSharp.Interop.ComProvider -Pre</pre>
</div>

Sample
------
Here is a simple script that uses COM to launch a Windows Explorer window:

*)
#r "FSharp.Interop.ComProvider.dll"

type Shell = TypeLib.``Microsoft Shell Controls And Automation``.``1.0``

let shell = Shell.ShellClass()
shell.Explore(@"C:\")
(**

If you're writing compiling to an assembly, you'll want to include the
`STAThread` attribute on your entry function for proper COM functionality:

*)
[<EntryPoint>]
[<STAThread>]
let main argv =
    let shell = Shell.ShellClass()
    shell.Explore(@"C:\")
    0
(**

Contributing and license
------------------------

The project is hosted on [GitHub][gh] where you can [report issues][issues], fork
the project and submit pull requests. If you're adding new public API, please also
consider adding [samples][content] that can be turned into a documentation. You might
also want to read [library design notes][readme] to understand how it works.

The library is available under a public domain license, which allows modification and
redistribution for both commercial and non-commercial purposes. For more information
see the [License file][license] in the GitHub repository.

  [content]: https://github.com/fsprojects/FSharp.Interop.ComProvider/tree/master/docs/content
  [gh]: https://github.com/fsprojects/FSharp.Interop.ComProvider
  [issues]: https://github.com/fsprojects/FSharp.Interop.ComProvider/issues
  [readme]: https://github.com/fsprojects/FSharp.Interop.ComProvider/blob/master/README.md
  [license]: https://github.com/fsprojects/FSharp.Interop.ComProvider/blob/master/LICENSE.txt
*)
