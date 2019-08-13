# Kaleidoscope Tutorial code in F#

This is my take on the [Kaleidoscope Tutorial](http://releases.llvm.org/8.0.1/docs/tutorial/index.html) in F#.

Current Status:

 - [x] Chapter 1 - Lexing : FsLex 9.0.2 for the lexer (see <Lexer.fsl>)
 - [x] Chapter 2 - Parrsing: FsYacc 9.0.2 for the parser (see <Parser.fsy>)
 - [x] Chapter 3 - LLVM IR Generation: LLVMSharp 8.0.0 (see <Codegen.fs>)

How to build: this is a dotnet SDK project, so the standard incantations work:

 - `dotnet run kaleidoscope.fsproj` to run
 - `dotnet test tests.fsproj` to test

How to build .exes: The dependency `libllvm` does not have a generic `linux-x64` package, so we need distro-specific runtime identifiers on Linux

 - `dotnet publish --project kaleidoscope.fsproj -r win-x64 -c Release` for Windows release
 - `dotnet publish --project kaleidoscope.fsproj -r ubuntu.18.04-x64 -c Release` for Ubuntu release
 
