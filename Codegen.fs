module Codegen

open LLVMSharp
open Parser
open System.Collections.Generic

let context = LLVMContextRef.Global
let themodule = context.CreateModuleWithName "module"
let builder = context.CreateBuilder()

let name_values = Dictionary<_, _>()

let rec codegen expr =
  let (!) = codegen
  let (!!) = List.map codegen >> List.toArray

  match expr with
  | Expr.Number f -> LLVMValueRef.CreateConstReal(context.DoubleType, f)
  | Variable n -> name_values.[n]
  | Binop((op, _), rhs, lhs) ->
    match op with
      | "+" -> builder.BuildFAdd(!lhs, !rhs)
      | "-" -> builder.BuildFSub(!lhs, !rhs)
      | "*" -> builder.BuildFMul(!lhs, !rhs)
      | "<" ->
         let bool = builder.BuildFCmp(LLVMRealPredicate.LLVMRealULT, !lhs, !rhs)
         builder.BuildUIToFP(bool, context.DoubleType)
  | Call(callee, args) -> builder.BuildCall(!callee, !! args)
  | Func(name, param, body) ->
    let func =
      let typ = [| for p in param -> context.DoubleType |]
      let ty = LLVMTypeRef.CreateFunction(context.DoubleType, typ)
      themodule.AddFunction(name, ty)
    name_values.[name] <- func
    for i, p in List.indexed param do name_values.[p] <- func.GetParam (uint32 i)
    let bb = func.AppendBasicBlock "entry"
    builder.PositionAtEnd bb
    let _ = builder.BuildRet (! body)
    func
  | Extern(name, param) ->
    let func =
      let typ = [| for p in param -> context.DoubleType |]
      let ty = LLVMTypeRef.CreateFunction(context.DoubleType, typ)
      themodule.AddFunction(name, ty)
    name_values.[name] <- func
    func
    
let dump_ir expr =
  let value = codegen expr
  value.Dump()
  printfn ""
