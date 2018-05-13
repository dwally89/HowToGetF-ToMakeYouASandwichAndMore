module TypeProviders2

open FSharp.Data

type iTunesResult = JsonProvider<"https://itunes.apple.com/search?term=The+Beatles">    

let PrintAllAlbums artist = 
    (sprintf "https://itunes.apple.com/search?term=%s" artist 
    |> iTunesResult.Load).Results
    |> Seq.map (fun r -> r.CollectionName)
    |> Seq.distinct
    |> Seq.choose id
    |> Seq.iter (fun album -> printfn "%s" album)
    