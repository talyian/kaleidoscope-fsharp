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
  // Examples from chapter 2
  // parse "def foo(x y) x+foo(y, 4.0);"
  // parse "def foo(x y) x+y y;"
  // parse "def foo(x y) x+y );"
  // parse "extern sin(a);"

  // Examples from chapter 3
  // parse "4 + 5;"
  // parse "def foo(a b) a*a + 2*a*b + b*b;"
  // parse "def bar(a) foo(a, 4.0) + bar(31337);"
  let rec loop s = 
    printf "kaleidoscope> "
    match System.Console.ReadLine() with
      | null -> 0
      | s -> parse s; loop s
  loop ""
