﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Expresser.Benchmarks
{
	[RPlotExporter]
	[SimpleJob(RuntimeMoniker.NetCoreApp22)]
	public class BasicBenchmarks
	{
		[Params("1 + (10 / 40) > 1 == true")]
		public string Expression { get; set; } = string.Empty;

		private SimpleMathExpression? evaluateTarget;

		[GlobalSetup]
		public void Setup()
		{
			evaluateTarget = SimpleMathExpression.Compile(Expression);
		}

		[Benchmark]
		public void Compile() => SimpleMathExpression.Compile(Expression);

		[Benchmark]
		public void Evaluate() => evaluateTarget!.Evaluate();
	}
}
