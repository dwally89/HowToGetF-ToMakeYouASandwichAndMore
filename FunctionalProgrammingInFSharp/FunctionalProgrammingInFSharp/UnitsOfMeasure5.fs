module UnitsOfMeasure5

[<Measure>] type kg
[<Measure>] type m
[<Measure>] type s

let calculateSpeed (distance:float<m>) (time:float<s>) = 
    distance / time

let calculateAcceleration (initialSpeed:float<m/s>) (finalSpeed:float<m/s>) (time:float<s>) = 
    (finalSpeed - initialSpeed) / time
    
let calculateForce (mass:float<kg>) (acceleration:float<m/s^2>) = 
    mass * acceleration
    
let example() = 
    let initialSpeed = calculateSpeed 10.0<m> 2.5<s>
    let acceleration  = calculateAcceleration initialSpeed 20.0<m/s> 30.1<s>
    let force = calculateForce 50.2<kg> acceleration
    0