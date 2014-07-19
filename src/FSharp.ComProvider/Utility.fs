module private FSharp.ComProvider.Utility

open System.Collections.Generic
open Microsoft.Win32

let memoize f =
    let cache = Dictionary<_, _>()
    fun x -> match cache.TryGetValue x with
             | true, value -> value
             | _ -> let value = f x
                    cache.[x] <- value
                    value

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
