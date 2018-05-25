module PatternMatching
open UnionTypes1
let PrintShape shape = 
    match shape with
    | Line(length) -> 
        sprintf "This is a line of length %i" length
    | Square(width, height) -> 
        sprintf "This is a square of dimensions %i,%i" width height
