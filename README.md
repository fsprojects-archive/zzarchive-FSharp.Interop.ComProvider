# COM Type Provider

The COM Type Provider provides a new way to do COM interop from F#.

For more details see the [documentation](http://fsprojects.github.io/FSharp.ComProvider/).

## Technical Overview

Normally, to do COM interop from a .NET project, you use the _Add Reference_
function of Visual Studio and select the COM component you would like to
reference. This generates an assembly containing the interop types that you can
then consume from your code.

Behind the scenes, _Add Reference_ actually depends on the `TypeLibConverter`
class to create the interop types (this is also used by the `tlbimp.exe` tool).
This allows us to leverage the same `TypeLibConverter` class from the type
provider to do the heavy work of generating the interop types.

## Limitations and Known Issues

The following known issues and limitations currently apply to the COM provider.
Some of them I would like to eventually rectify if possible:

* Only 32-bit target executables and COM libraries are supported.
* Type libraries with Primary Interop Assemblies (PIAs) such as Microsoft
  Office are not supported.
* All the types generated from the type library are embedded, rather than
  just the ones you refer to in your code.
