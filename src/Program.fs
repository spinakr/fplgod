module Program

open Suave
open Suave.Filters
open Suave.Operators

let app = 
    choose [
        GET >=> choose [
            pathScan "/player/stats/%s" (fun name -> 
                (SeasonStats.getPlayerStatsByName >> string >> Successful.OK) name)
            pathScan "/player/history/%i" (fun id -> 
                (HistoricStats.getPlayerPreviouseGw >> string >> Successful.OK) id)
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
