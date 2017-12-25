module Program

open Suave
open Suave.Filters
open Suave.Operators
open SeasonStats
open HistoricStats

let app = 
    choose [
        GET >=> choose [
            pathScan "/player/stats/%s" (fun name -> 
                (getPlayerStatsByName >> string >> Successful.OK) name)
            pathScan "/player/history/%i" (fun id -> 
                (getPlayerPreviouseGw >> string >> Successful.OK) id)
        ]
    ]

[<EntryPoint>]
let main argv =
    let config =
        { defaultConfig with 
            bindings = [ HttpBinding.createSimple HTTP "0.0.0.0" 8080 ]
        }
    
    startWebServer config app
    0
