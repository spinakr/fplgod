open Suave
open Suave.Filters
open Newtonsoft.Json
open Suave.Operators
open SeasonStats


[<EntryPoint>]
let main argv =
    let app = 
        choose [
            GET >=> choose [
                pathScan "/player/%s" (fun name -> 
                    (getPlayerStatsByName >> JsonConvert.SerializeObject >> Successful.OK) name)
            ]
        ]

    let config =
        { defaultConfig with 
            bindings = [ HttpBinding.createSimple HTTP "0.0.0.0" 8080 ]
        }
    
    startWebServer config app
    0
