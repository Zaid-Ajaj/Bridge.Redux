open System.IO


let rec getFiles dir = seq { 

    for file in Directory.GetFiles(dir) do
        yield file
    for subdir in Directory.GetDirectories(dir) do
        yield! (getFiles subdir)
}


let libraryName = "Redux"
let usedNamespace = "Bridge.Redux"
let destinationFile = sprintf "Generated.%s.cs" libraryName
let destinationPath =  __SOURCE_DIRECTORY__ + "\\" + destinationFile

let getSourceFiles() =
    getFiles __SOURCE_DIRECTORY__
    |> Seq.filter (fun file -> file.EndsWith(".cs"))
    |> Seq.filter (fun file -> not (file.Contains(destinationFile)))
    |> Seq.filter (fun file -> not (file.Contains("AssemblyInfo.cs")))
    |> Seq.filter (fun file -> not (file.Contains("obj\Debug")))
    |> List.ofSeq



let getUsingsOnly() =
    getSourceFiles()
    |> Seq.map File.ReadAllText
    |> Seq.map (fun file -> file.Split('\n'))
    |> Seq.concat
    |> Seq.where (fun line -> line.StartsWith("using"))
    |> Seq.map (fun line -> line.Trim())
    |> Seq.distinct
    |> List.ofSeq
    |> String.concat "\n"

let getFilesWithoutUsings() = 
     getSourceFiles()
    |> Seq.map File.ReadAllText
    |> Seq.map (fun file -> file.Split('\n'))
    |> Seq.concat
    |> List.ofSeq
    |> Seq.where (fun file -> not (file.StartsWith("using")))
    |> String.concat "\n"
    



[ 
    "using Bridge;"
    "using System;"
    getUsingsOnly()
    getFilesWithoutUsings()
]
|> String.concat "\n"
|> fun source -> source.Replace(usedNamespace, libraryName)
|> fun source -> File.WriteAllText(__SOURCE_DIRECTORY__ + "\\" + destinationFile, source)