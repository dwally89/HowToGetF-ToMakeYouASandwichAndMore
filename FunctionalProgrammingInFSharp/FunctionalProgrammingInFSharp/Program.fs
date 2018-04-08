open System.IO
open System.Text.RegularExpressions

let processLine line = 
    let mat = Regex.Match(line, "{{ID_(.*)}}")
    if mat.Success then
        let directory = "C:\Users\dwall\Repositories\FunctionalProgrammingInF#\FunctionalProgrammingInFSharp\FunctionalProgrammingInFSharp"
        let filename = mat.Groups.[1].Value
        Path.Combine(directory, sprintf "%s.fs" filename)
        |> File.ReadAllLines
        |> Array.skip 2
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
