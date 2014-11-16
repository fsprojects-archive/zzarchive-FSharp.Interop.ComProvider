module private FSharp.Interop.ComProvider.TypeLibInfo

open System
open Microsoft.Win32
open Utility

type TypeLibVersion = {
    String: string;
    Major: int;
    Minor: int }

type TypeLib = {
    Name : string;
    Version: TypeLibVersion;
    Platform: string;
    Path: string }

let private tryParseVersion (text:string) =
    match text.Split('.') |> Array.map Int32.TryParse with
    | [|true, major; true, minor|] -> Some { String = text; Major = major; Minor = minor }
    | _ -> None

let loadTypeLibs preferredPlatform =
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
             && versionKey.GetValue("PrimaryInteropAssemblyName") = null
          then
              yield { Name = name
                      Version = version.Value
                      Platform = platformKey.SubKeyName
                      Path = platformKey.DefaultValue } ]
    |> Seq.groupBy (fun lib -> lib.Name, lib.Version)
    |> Seq.map (fun (_, libs) ->
        match libs |> Seq.tryFind (fun lib -> lib.Platform = preferredPlatform) with
        | Some lib -> lib
        | None -> Seq.head libs)
