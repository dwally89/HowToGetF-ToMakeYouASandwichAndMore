module UnitsOfMeasure2
open UnitsOfMeasure1
let AddAmounts (x:int<USD>) (y:int<USD>) = 
    x + y

let TestAdder() = 
    AddAmounts 1<USD> 2<USD>

(*FAKECOMMENT
let TestAdderInvalid() = 
    AddAmounts 1<USD> 2
FAKECOMMENT*)