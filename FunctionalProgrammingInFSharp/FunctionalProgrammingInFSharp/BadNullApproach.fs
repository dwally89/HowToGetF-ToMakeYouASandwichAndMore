module BadNullApproach
open Person2
let PrintPerson person = 
    printfn "Name:%s, Age:%i" person.Name person.Age

(*FAKECOMMENT
let NullExample() = 
    PrintPerson null
FAKECOMMENT*)

let RealExample() = 
    PrintPerson {Name="Bob"; Age=52;}