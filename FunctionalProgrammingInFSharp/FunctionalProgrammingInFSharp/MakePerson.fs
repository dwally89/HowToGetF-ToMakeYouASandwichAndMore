module MakePerson

type Person = {Name:string; Age:int;}

let MakePerson name =
    {Name=name; Age=0;}