#r "../packages/FSharp.Data/lib/net45/FSharp.Data.dll"
#r "../packages/Suave/lib/netstandard1.6/Suave.dll"
#r "../packages/Newtonsoft.Json/lib/net45/Newtonsoft.Json.dll"
#load "SeasonStats.fs"
#load "HistoricStats.fs"
#load "Program.fs"

open Suave
open System
open System.Threading
let cts = new CancellationTokenSource()
let conf = { defaultConfig with cancellationToken = cts.Token }
let listening, server = startWebServerAsync conf (Program.app)

Async.Start(server, cts.Token)
Console.ReadKey true |> ignore

cts.Cancel()
0