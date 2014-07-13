module private ComProvider.TypeLibInfo

open System
open Microsoft.Win32
open ComProvider.Utility

type TypeLibVersion = { 
    String: string; 
    Major: int; 
    Minor: int }

type TypeLib = { 
    Name : string; 
    Version: TypeLibVersion;
    Path: string }

let private tryParseVersion (text:string) = 
    match text.Split('.') |> Array.map Int32.TryParse with
    | [|true, major; true, minor|] -> Some { String = text; Major = major; Minor = minor }
    | _ -> None

let loadTypeLibs() =
    [ use rootKey = Registry.ClassesRoot.OpenSubKey("TypeLib")
      for typeLibKey in rootKey.GetSubKeys() do
      for versionKey in typeLibKey.GetSubKeys() do
      for localeKey in versionKey.GetSubKeys() do
      for platformKey in localeKey.GetSubKeys() do
          let name = versionKey.DefaultValue
          let version = tryParseVersion versionKey.SubKeyName
          if name <> ""
             && version.IsSome 
             && localeKey.SubKeyName = "0" 
             && platformKey.SubKeyName = "win32"
             && versionKey.GetValue("PrimaryInteropAssemblyName") = null 
          then
              yield { Name = name
                      Version = version.Value
                      Path = platformKey.DefaultValue } ]
    |> Seq.distinct
