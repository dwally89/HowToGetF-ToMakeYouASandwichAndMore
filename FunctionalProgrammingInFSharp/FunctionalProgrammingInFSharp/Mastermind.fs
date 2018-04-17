

namespace Mastermind

open System

type Result<'a,'b> = 
    |ValidGuess of 'a
    |InvalidGuess of 'b

type MatchResult = 
    |Correct
    |InWrongPlace
    |Incorrect

module CodeComparer =    
    let updateMatchedItems i = 
        Seq.mapi (fun idx existingItem -> if idx = i then true else existingItem)

    let findItemInWrongPlace item matchedItems = 
        Seq.mapi (fun cidx citem -> cidx, citem)
        >> Seq.filter (fun (cidx, citem) -> (not <| Seq.item cidx matchedItems) && citem = item)
        >> Seq.map fst
        >> Seq.tryHead

    let compareOne code acc pair = 
        let existingResults, matchedItems = acc
        let i, item = pair
        let result, newMatchedItems = 
                    if item = Seq.item i code then
                        Correct, (matchedItems |> updateMatchedItems i)
                    else
                        match code |> findItemInWrongPlace item matchedItems with
                        |Some(matchedI) -> InWrongPlace, (matchedItems |> updateMatchedItems matchedI)
                        |None -> Incorrect, matchedItems                     
        
        (List.append existingResults [result]), newMatchedItems

    let getAllInCorrectPlace guess code = 
        guess |> Seq.mapi (fun idx item -> Seq.item idx code = item)

    let compare guess code = 
        let codeLength = Seq.length code
        if Seq.length guess <> codeLength then
            InvalidGuess(sprintf "Incorrect guess length. Expected length: %i" codeLength)
        else
            let matchedItems = getAllInCorrectPlace guess code
            guess 
            |> Seq.mapi (fun i item -> i, item)
            |> Seq.fold (compareOne code) ([], matchedItems)
            |> fst
            |> ValidGuess

    let compareAndPrintResult guess code = 
        match compare guess code with
        |ValidGuess(result) -> printfn "%A , %A = %A" guess code result
        |InvalidGuess(f) -> printfn "%A , %A = %s" guess code f

module Game = 
    let compareResults result1 result2 = 
        match result1 with
        | ValidGuess(m) ->
            let length = m |> Seq.length
            [0..length-1]
            |> Seq.forall (fun i -> Seq.item i m = Seq.item i result2)
        | InvalidGuess(e) -> false

    let rec start code remainingGuesses = 
        let guess = remainingGuesses |> Seq.head
        let result = CodeComparer.compare guess code
        match result with
        | ValidGuess(m) -> 
            if m |> Seq.forall (fun r -> match r with 
                                         |Correct -> true
                                         | _ -> false) then
                guess
            else
                remainingGuesses
                |> Seq.skip 1
                |> Seq.filter (fun g -> let r = (CodeComparer.compare g guess)
                                        compareResults r m)
                |> start code
        | InvalidGuess(e) -> invalidOp e

    let rec generateGuesses length (options:list<'a>) (existingGuesses:list<list<'a>>) = 
        if length = 0 then
            existingGuesses
        else
            options
            |> List.collect (fun o -> if Seq.isEmpty existingGuesses then
                                          [[o]]
                                      else
                                          existingGuesses |> List.map (fun g -> List.append [o] g))
            |> generateGuesses (length-1) options

    let playGame() = 
        let options = [1..6]        
        let length = 4
        let random = new Random()
        let code = [1..length] |> List.map (fun i -> Seq.item (random.Next(Seq.length options)) options)        
        printfn "%A" code
        let allGuesses = generateGuesses length options []
        printfn "%A" allGuesses
        start code allGuesses        
