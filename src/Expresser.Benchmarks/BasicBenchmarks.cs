using BenchmarkDotNet.Attributes;

namespace Expresser.Benchmarks
{
	[CoreJob]
	[RPlotExporter]
	public class BasicBenchmarks
	{
		[Params ("1 + (10 / 40) > 1 == true", "1+1", "true == false")]
		public string Expression { get; set; }

		private CompiledExpression EvaluateTarget;
		private ExpressionSyntax CompileSyntax;

		[GlobalSetup]
		public void Setup ()
		{
			EvaluateTarget = CompiledExpression.Compile (Expression);
			CompileSyntax = new ExpressionSyntax (Expression);
		}

		[Benchmark]
		public void Parse () => new ExpressionSyntax (Expression);

		[Benchmark]
		public void Compile () => CompiledExpression.Compile (CompileSyntax);

		[Benchmark]
		public void Evaluate () => EvaluateTarget.Evaluate ();
	}
}
