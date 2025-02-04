﻿namespace Tests.Patterns

open System
open Stringier.Patterns
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type CompoundPatternTests() =

    [<TestMethod>]
    member _.``alternate repeater`` () =
        let pattern = ("Hi " || "Bye ") * 2;
        ResultAssert.Captures("Hi Bye ", pattern.Consume("Hi Bye Hi"))

    [<TestMethod>]
    member _.``alternate spanner`` () =
        let pattern = many (Pattern.SpaceSeparator || "\t")
        ResultAssert.Captures("  \t ", pattern.Consume("  \t "))

    [<TestMethod>]
    member _.``optional spanner`` () =
        let pattern = kleene ' '
        ResultAssert.Captures("  ", pattern.Consume("  Hello"))
        ResultAssert.Captures("", pattern.Consume("Hello"))

    [<TestMethod>]
    member _.``spaning optor`` () =
        Assert.ThrowsException<PatternConstructionException>(fun () -> many(maybe ' ') |> ignore) |> ignore