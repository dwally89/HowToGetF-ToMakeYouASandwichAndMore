module Person2

type Person = {Name:string; Age:int;}

let MakePerson name =
    {Name=name; Age=0;}

// Create new person by just modifying the age
let GrowOlder person = 
    {person with Age=person.Age+1}
        
printfn "Hello, World!"
let person1 = {Name="Bob";Age=52;}
let person2 = {Name="Bob";Age=52;}

// Print to console in a type-safe way
printfn "%i" (person1.GetHashCode())
printfn "%i" (person2.GetHashCode())
printfn "%b" (person1=person2)