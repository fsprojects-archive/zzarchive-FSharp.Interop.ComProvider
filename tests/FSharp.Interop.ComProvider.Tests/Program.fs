open System

type Shell = COM.``Microsoft Shell Controls And Automation``.``1.0``

[<EntryPoint; STAThread>]
let main argv =
    let shell = Shell.ShellClass()
    shell.Open @"C:\"
    0
