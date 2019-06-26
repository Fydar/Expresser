using BenchmarkDotNet.Attributes;

namespace Expresser.Benchmarks
{
	[CoreJob]
	[RPlotExporter]
	public class ComparedWithRaw
	{
		[Params ("1 + (10 / 40) > 1 == true")]
		public string Expression { get; set; }

		private CompiledExpression EvaluateTarget;
		private ExpressionSyntax CompileSyntax;

		private bool Result;

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

		[Benchmark]
		public void CSharp () => Result = 1 + (10 / 40) > 1 == true;
	}
}
