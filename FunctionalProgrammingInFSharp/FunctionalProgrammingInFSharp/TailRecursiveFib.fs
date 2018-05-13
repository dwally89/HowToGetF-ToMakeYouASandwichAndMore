module TailRecursiveFib

let rec fib n a b =
    match n with
    | 0 -> a
    | 1 -> b
    | _ -> fib (n-1) b (a+b)