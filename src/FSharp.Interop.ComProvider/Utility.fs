module private FSharp.Interop.ComProvider.Utility

open Microsoft.Win32
open System.Reflection
open System

type RegistryKey with
    member this.SubKeyName =
        this.Name.Split '\\' |> Seq.last
    member this.DefaultValue =
        this.GetValue("") |> string
    member this.GetSubKeys() =
        seq {
            for keyName in this.GetSubKeyNames() do
                use key = this.OpenSubKey keyName
                yield key
        }

type ICustomAttributeProvider with
    member this.TryGetAttribute<'t when 't :> Attribute>() =
        this.GetCustomAttributes(typeof<'t>, true)
        |> Seq.cast<'t>
        |> Seq.tryFind (fun _ -> true)
