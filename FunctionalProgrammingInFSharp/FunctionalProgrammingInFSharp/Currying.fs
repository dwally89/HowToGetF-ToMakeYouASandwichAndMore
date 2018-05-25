module Currying

let add x y = x + y
let multiply x y = x * y

let add5 = add 5
let multiply7 = multiply 7

let add5ThenMultiplyBy7 = add5 >> multiply7

let printResult = printfn "%i %s %i = %i"

let printAddResult x y = printResult x "+" y

let addAndPrintResult x y =
    add x y
    |> printAddResult x y

let example() = 
    let result1 = add5ThenMultiplyBy7 15
    addAndPrintResult 3 9