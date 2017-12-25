#r "../packages/FSharp.Data/lib/net45/FSharp.Data.dll"

open FSharp.Data

type Players = JsonProvider<"../data-files/fpl-data.json">
type Player = Players.Element

type PlayerHistory = JsonProvider<"../data-files/player-data.json">

type PlayerStats = {
    Name: string
    TotalPoints: int
    PreviousGwPoints: int
}

let populateWithPlayerStats (player:Player) = 
    let playerHistoryUrl = (sprintf "https://fantasy.premierleague.com/drf/element-summary/%i" player.Id)
    let playerHistory = PlayerHistory.Load(playerHistoryUrl)
    let previousGw = Seq.last playerHistory.History

    {Name=player.WebName; 
     TotalPoints=player.TotalPoints; 
     PreviousGwPoints=previousGw.TotalPoints}

let playersStats = Players.Load("https://fantasy.premierleague.com/drf/bootstrap-static")
let players = 
    playersStats.Elements 
    |> Seq.sortByDescending (fun x -> x.TotalPoints)
    |> Seq.take 10
    |> Seq.map populateWithPlayerStats

players |> Seq.map (fun x ->
    printfn "Name: %s - Total Points: %i - GW Points: %i" x.Name x.TotalPoints x.PreviousGwPoints
)

