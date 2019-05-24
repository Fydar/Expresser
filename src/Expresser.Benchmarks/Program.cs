using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System;

namespace Expresser.Benchmarks
{
	[CoreJob]
	[RPlotExporter, RankColumn]
	public class BasicBenchmarks
	{
		[Params ("1 + (10 / 40) > 1 == true", "1+1", "true == false")]
		public string Expression { get; set; }

		private CompiledExpression EvaluateTarget;
		private ExpressionSyntax CompileSyntax;

		[GlobalSetup]
		public void Setup ()
		{
			EvaluateTarget = new CompiledExpression (Expression);
			CompileSyntax = new ExpressionSyntax (Expression);
		}

		[Benchmark]
		public void ParseSyntax () => new ExpressionSyntax (Expression);

		[Benchmark]
		public void Compile () => new CompiledExpression (CompileSyntax);

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
