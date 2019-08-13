// Learn more about F# at http://fsharp.org

open System

let parse source =
  printfn "    %s" source
  let buf = FSharp.Text.Lexing.LexBuffer<_>.FromString source
  try
    Parser.start (Lexer.tokenize) buf
  with
    | :? Parser.ParseError as e ->
      let (tok, t1, t2) = e.Data
      printfn "Error: %O" tok
      printfn "Expected: %O" [for t in t1 @ t2 -> Parser.tokenTagToTokenId t]
[<EntryPoint>]
let main argv =
  parse "def foo(x y) x+foo(y, 4.0);"
  parse "def foo(x y) x+y y;"
  parse "def foo(x y) x+y );"
  parse "extern sin(a);"
  0 // return an integer exit code
