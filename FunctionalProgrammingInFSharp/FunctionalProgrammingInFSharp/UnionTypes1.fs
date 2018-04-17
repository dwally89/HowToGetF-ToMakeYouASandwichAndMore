module UnionTypes1

type Shape =
| Line of int
| Square of width:int * height:int
| Triangle of int * int * int
| Cube of int * int * int