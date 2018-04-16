

namespace FSharp

open System

module Parser = 
    let TryParseInt value = 
        try
            Some(Int32.Parse value)
        with
        | _ -> None

    let ReadInt() = 
        let line = Console.ReadLine()
        match TryParseInt line with
        | Some i -> printfn "Successfully read integer: %i" i
        | None -> printfn "Failed to read integer"