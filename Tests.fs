module Kaleidoscope.Tests
open Microsoft.VisualStudio.TestTools.UnitTesting

let parse s =
  let buf = FSharp.Text.Lexing.LexBuffer<_>.FromString s
  Parser.start (Lexer.tokenize) buf

let codegen s =
  parse s |> List.head |> Codegen.codegen |> string

[<TestClass>]
type Chapter2 () =
  [<DataTestMethod>]
  [<DataRow("def foo(x y) x+foo(y, 4.0);")>]
  [<DataRow("def foo(x y) x+y y;")>]
  [<DataRow("extern sin(a);")>]
  member __.ParseSuccess(source:string) =
    Assert.IsNotNull(parse source)

  [<DataTestMethod>]
  [<DataRow("def foo(x y) x+y );")>]
  [<ExpectedException(typeof<Parser.ParseError>)>]
  member __.ParseError(source:string) =
    Assert.IsNotNull(parse source)

[<TestClass>]
type Chapter3 () =
  [<DataTestMethod>]
  [<DataRow("4 + 5;")>]
  [<DataRow("def foo(a b) a*a + 2*a*b + b*b;")>]
  [<DataRow("def bar(a) foo(a, 4.0) + bar(31337);")>]
  member __.Codegen1(source) =
    Assert.IsNotNull(codegen source)
