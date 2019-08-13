let parse source =
  let buf = FSharp.Text.Lexing.LexBuffer<_>.FromString source
  try
    let expr = Parser.start (Lexer.tokenize) buf
    List.map Codegen.dump_ir expr |> ignore
  with
    | :? Parser.ParseError as e ->
      let (tok, t1, t2) = e.Data
      printfn "Error: %O" tok
      printfn "Expected: %O" [for t in t1 @ t2 -> Parser.tokenTagToTokenId t]
    | e -> printfn "Error: %O" e.Message

[<EntryPoint>]
let main argv =
  let rec loop s = 
    printf "kaleidoscope> "
    match System.Console.ReadLine() with
      | null -> 0
      | s -> parse s; loop s
  loop ""
