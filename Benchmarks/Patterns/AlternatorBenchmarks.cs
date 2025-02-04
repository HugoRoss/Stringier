﻿using System;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using PCRE;
using Pidgin;
using static Pidgin.Parser;
using static Pidgin.Parser<char>;
using Stringier.Patterns;

namespace Benchmarks.Patterns {
	[SimpleJob(RuntimeMoniker.Net48)]
	[SimpleJob(RuntimeMoniker.NetCoreApp30)]
	[SimpleJob(RuntimeMoniker.CoreRt30)]
	[SimpleJob(RuntimeMoniker.Mono)]
	[MemoryDiagnoser]
	public class AlternatorBenchmarks {

		readonly Regex msregex = new Regex("^(?:Hello|Goodbye)");

		readonly Regex msregexCompiled = new Regex("^(?:Hello|Goodbye)", RegexOptions.Compiled);

		readonly PcreRegex pcreregex = new PcreRegex("^(?:Hello|Goodbye)");

		readonly PcreRegex pcreregexCompiled = new PcreRegex("^(?:Hello|Goodbye)", PcreOptions.Compiled);

		readonly Parser<Char, String> pidgin = String("Hello").Or(String("Goodbye"));

		readonly Pattern stringier = "Hello".Or("Goodbye");

		[Params("Hello", "Goodbye", "Failure")]
		public String Source { get; set; }

		[Benchmark]
		public Match MSRegex() => msregex.Match(Source);

		[Benchmark]
		public Match MSRegexCompiled() => msregexCompiled.Match(Source);

		[Benchmark]
		public PcreMatch PcreRegex() => pcreregex.Match(Source);

		[Benchmark]
		public PcreMatch PcreRegexCompiled() => pcreregexCompiled.Match(Source);

		[Benchmark]
		public Result<Char, String> Pidgin() => pidgin.Parse(Source);

		[Benchmark]
		public Result Stringier() => stringier.Consume(Source);
	}
}
