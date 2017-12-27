module HistoricStats

open FSharp.Data
open System
open FSharp.Data.Runtime.Caching

type PlayerHistory = JsonProvider<"../data-files/player-data.json">

let ttl = TimeSpan.FromMinutes(5.)
let cache, cacheDirectory = createInternetFileCache "playerHistory" ttl

let loadPlayerHistory (id:int) =
    let playerHistoryUrl = (sprintf "https://fantasy.premierleague.com/drf/element-summary/%i" id)
    let playerHistory = PlayerHistory.Load playerHistoryUrl
    cache.Set ((string id), (string playerHistory))
    playerHistory

let getPlayerHistory id = 
    match (cache.TryRetrieve (string id)) with
    | Some value -> PlayerHistory.Parse value
    | None -> loadPlayerHistory id

let getPlayerPreviouseGw (id:int) =
    let playerHistory = getPlayerHistory id
    Seq.last playerHistory.History


