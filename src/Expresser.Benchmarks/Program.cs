using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;

namespace Expresser.Benchmarks
{
	[CoreJob]
	[RPlotExporter, RankColumn]
	public class BasicBenchmarks
	{
		private CompiledExpression EvaluateTarget;

		[GlobalSetup]
		public void Setup ()
		{
			EvaluateTarget = new CompiledExpression ("1 + (10 / 40) > 1 == true");
		}

		[Benchmark]
		public void ParseSyntax () => new ExpressionSyntax ("1 + (10 / 40) > 1 == true");

		[Benchmark]
		public void Evaluate () => EvaluateTarget.Evaluate ();
	}

	public class Program
	{
		public static void Main (string[] args)
		{
			var summary = BenchmarkRunner.Run<BasicBenchmarks> ();

			Console.WriteLine (summary);
		}
	}
}
