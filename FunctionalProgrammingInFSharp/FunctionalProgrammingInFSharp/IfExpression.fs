module IfExpression

let isInTheBeatles name = ["John"; "Paul"; "George"; "Ringo"] |> Seq.contains name

let moreComplicatedFunction name = false

let analyseDrummer drummerName = 
    let isTheGreatest = 
        if drummerName = "Ringo" then
            if isInTheBeatles drummerName then
                false
            else
                moreComplicatedFunction drummerName
        else
            moreComplicatedFunction drummerName
    if isTheGreatest then
        printfn "%s is the greatest drummer" drummerName