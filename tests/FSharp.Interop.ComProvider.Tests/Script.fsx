#I "../../bin"
#r "FSharp.Interop.ComProvider.dll"

type Shell = COM.``Microsoft Shell Controls And Automation``.``1.0``

let shell = Shell.ShellClass()
shell.Open @"C:\"
