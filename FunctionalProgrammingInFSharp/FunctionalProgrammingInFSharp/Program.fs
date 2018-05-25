open System.IO
open System.Text.RegularExpressions

type Result<'TSuccess, 'TError> =
| Success of 'TSuccess
| Error of 'TError 

let shouldKeepLine (line:string) = 
    not <| line.StartsWith "(*FAKECOMMENT" &&
    not <| line.StartsWith("FAKECOMMENT*)")

let processCodeLine (line:string) = 
    line.Replace("<", "&lt").Replace(">", "&gt")

let rec processLines solutionDir fSharpDirectory cSharpDirectory alreadyProcessedLines remainingLines = 
    if remainingLines |> Seq.isEmpty then 
        alreadyProcessedLines |> Success
    else
        let line = remainingLines |> Seq.head
        let rest = remainingLines |> Seq.tail
        let mat = Regex.Match(line, "{{ID_(.*)}}")
        if mat.Success then
            let filename = mat.Groups.[1].Value
            let path = if filename.EndsWith "CSharp" then 
                           Path.Combine(solutionDir, cSharpDirectory, sprintf "%s.cs" (filename.Replace("_CSharp", "")))
                       else
                           Path.Combine(solutionDir, fSharpDirectory, sprintf "%s.fs" filename)
            if path |> File.Exists |> not then 
                sprintf "Failed to find file %s" path
                |> Error
            else
                let fileLines = path
                                |> File.ReadAllLines
                                |> Seq.skip 2
                                |> Seq.filter shouldKeepLine
                                |> Seq.map processCodeLine
                let newProcessedLines = Seq.append alreadyProcessedLines fileLines
                processLines solutionDir fSharpDirectory cSharpDirectory newProcessedLines rest
        else
            let newProcessedLines = Seq.append alreadyProcessedLines [|line|]
            processLines solutionDir fSharpDirectory cSharpDirectory newProcessedLines rest
            

[<EntryPoint>]
let main argv = 
    let directory = Directory.GetCurrentDirectory()
    let filename = "index"
    let extension = "html"
    let outputPath = Path.Combine(directory, sprintf "%s_processed.%s" filename extension)
    let inputFile = Path.Combine(directory, sprintf "%s.%s" filename extension)
    let solutionDir = Path.Combine(directory,  "FunctionalProgrammingInFSharp")
    if inputFile |> File.Exists |> not then 
        printfn "Failed to find input file %s" inputFile
        -1
    elif solutionDir |> Directory.Exists |> not then
        printfn "Failed to find solution directory %s" solutionDir
        -1
    else
        let fSharpDirectory = "FunctionalProgrammingInFSharp"
        let cSharpDirectory = "CSharpCode"
            
        let processed = inputFile
                        |> File.ReadAllLines
                        |> processLines solutionDir fSharpDirectory cSharpDirectory []
        match processed with
        | Success lines ->                
            printfn "Writing processed presentation to %s" outputPath
            File.WriteAllLines(outputPath, lines)
            printfn "Finished"
            0
        | Error error ->
            printfn "%s" error
            -1
