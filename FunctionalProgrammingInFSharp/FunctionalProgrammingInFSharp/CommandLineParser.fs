namespace Parser

open System

module CommandLineParser = 
    let GetArgument prompt parser = 
        printf "%s" prompt
        Console.ReadLine() |> parser

    let GetInteger() = 
        GetArgument "Enter an integer: " Int32.Parse