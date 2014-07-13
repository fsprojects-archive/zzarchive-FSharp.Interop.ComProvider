# COM Type Provider

The COM Type Provider provides a new way to do COM interop from F#. It allows you to access COM components directly from F# projects and scripts without adding any references other than the type provider itself.

One advantage of this method is that you can author and deploy F# scripts without having to pre-generate the interop assemblies. Another advantage is that you can easily explore all the COM components installed on your machine via intellisense:

![Visual Studio Intellisense Example](sample-dark.png)

Here is an example program that uses COM to launch a Windows Explorer window:

``` fsharp
open System

type Shell = TypeLib.``Microsoft Shell Controls And Automation``.``1.0``

[<EntryPoint>]
[<STAThread>]
let main argv =
    let shell = Shell.ShellClass()
    shell.Explore(@"C:\")
    0
```

Here is the same example as a script:

``` fsharp
#r @"ComProvider.dll"

open System

type Shell = TypeLib.``Microsoft Shell Controls And Automation``.``1.0``

let shell = Shell.ShellClass()
shell.Explore(@"C:\")
```

## Technical Details

Normally, to do COM interop from a .NET project, you use the _Add Reference_ function of Visual Studio and select the COM component you would like to reference. This generates an assembly containing the interop types that you then consume from your code.

Behind the scenes, _Add Reference_ actually depends on the `TypeLibConverter` class to create the interop types (this is also used by the `tlbimp.exe` tool). This allows us to leverage the same `TypeLibConverter` class from the type provider to do the heavy work of generating the interop types.

## Limitations

The following known issues and limitations currently apply to the COM provider. Some of them I would like to eventually rectify if possible:

* Only 32-bit target executables and COM libraries are supported.
* Type libraries with Primary Interop Assemblies (PIAs) such as Microsoft Office are not supported.
* All the types generated from the type library are embedded, rather than just the ones you refer to in your code.
* All types are embedded as public, directly inside the module or namespace where they are referenced.
