module SeasonStats

open FSharp.Data
open System

type SeasonStats = JsonProvider<"../data-files/fpl-data.json">
type PlayerSeasonStats = SeasonStats.Element

let cacheDuration = TimeSpan.FromMinutes(10.)
let mutable statsLoaded = DateTime.Now
let loadSeasonStats () =
    SeasonStats.Load("https://fantasy.premierleague.com/drf/bootstrap-static").Elements 
    |> Seq.cast<PlayerSeasonStats> 

let mutable seasonStats = loadSeasonStats()

let refreshSeasonStats () =
    printfn "Season stats refreshed"
    seasonStats <- loadSeasonStats()
    statsLoaded <- DateTime.Now

let getPlayerStatsBy propSelector id = 
    if DateTime.Now - statsLoaded > cacheDuration
    then refreshSeasonStats()
    seasonStats |> Seq.find (fun x -> propSelector x = id)

let getPlayerStatsById id = 
    getPlayerStatsBy (fun x -> x.Id) id

let getPlayerStatsByName name = 
    getPlayerStatsBy (fun x -> x.WebName) name