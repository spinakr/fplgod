#r "../packages/FSharp.Data/lib/net45/FSharp.Data.dll"
#r "../packages/Suave/lib/netstandard1.6/Suave.dll"
#r "../packages/Newtonsoft.Json/lib/net45/Newtonsoft.Json.dll"
#load "SeasonStats.fs"
#load "HistoricStats.fs"

open Suave
open System
open System.Threading
open Suave.Filters
open Newtonsoft.Json
open Suave.Operators
open SeasonStats

let app = 
    choose [
        GET >=> choose [
            pathScan "/player/%s" (fun name -> 
                (getPlayerStatsByName >> string >> Successful.OK) name)
        ]
    ]

let cts = new CancellationTokenSource()
let conf = { defaultConfig with cancellationToken = cts.Token }
let listening, server = startWebServerAsync conf (app)

Async.Start(server, cts.Token)
Console.ReadKey true |> ignore

cts.Cancel()
0

//getPlayerPreviouseGw 10

//getPlayerStatsById 10
//getPlayerStatsByName "Sterling"
