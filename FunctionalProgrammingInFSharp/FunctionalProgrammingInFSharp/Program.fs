open System.IO
open System.Text.RegularExpressions

let shouldKeepLine (line:string) = 
    not <| line.StartsWith "(*FAKECOMMENT" &&
    not <| line.StartsWith("FAKECOMMENT*)")

let processLine line = 
    let mat = Regex.Match(line, "{{ID_(.*)}}")
    if mat.Success then
        let solutionDir = "C:\Users\dwall\Repositories\FunctionalProgrammingInF#\FunctionalProgrammingInFSharp"
        let fSharpDirectory = "FunctionalProgrammingInFSharp"
        let cSharpDirectory = "CSharpCode"
        let filename = mat.Groups.[1].Value
        let path = if filename.EndsWith "CSharp" then 
                       Path.Combine(solutionDir, cSharpDirectory, sprintf "%s.cs" (filename.Replace("_CSharp", "")))
                   else
                       Path.Combine(solutionDir, fSharpDirectory, sprintf "%s.fs" filename)
        path
        |> File.ReadAllLines
        |> Seq.skip 2
        |> Seq.filter shouldKeepLine
        |> Seq.toArray
    else
        [|line|]

[<EntryPoint>]
let main argv = 
    let directory = "C:\Users\dwall\Repositories\FunctionalProgrammingInF#"
    let filename = "index"
    let extension = "html"
    let outputPath = Path.Combine(directory, sprintf "%s_processed.%s" filename extension)
    let processed = Path.Combine(directory, sprintf "%s.%s" filename extension)
                    |> File.ReadAllLines
                    |> Seq.collect processLine
    File.WriteAllLines(outputPath, processed)
    printfn "Finished"
    0
