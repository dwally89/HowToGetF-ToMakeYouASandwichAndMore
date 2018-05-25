module TypeProviders1

open FSharp.Data

// Infer type from sample JSON
type Person = JsonProvider<""" { "name":"John", "age":94} """>

// Parse real input
let thomas = Person.Parse """ { "name":"Thomas", "age":14 } """
printfn "%s" thomas.Name
printfn "%i" thomas.Age  