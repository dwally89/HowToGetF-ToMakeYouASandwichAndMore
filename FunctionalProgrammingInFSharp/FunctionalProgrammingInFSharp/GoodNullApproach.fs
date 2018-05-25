module GoodNullApproach
open Person2
let PrintPerson person =
    match person with
    | Some p -> printfn "%A" p
    | None -> printfn "No person given" 

let NullExample() = 
    PrintPerson None

let RealExample() =
    PrintPerson (Some({Name="Bob"; Age=52;}))