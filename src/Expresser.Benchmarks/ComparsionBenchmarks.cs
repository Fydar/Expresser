using BenchmarkDotNet.Attributes;
using CodingSeb.ExpressionEvaluator;

namespace Expresser.Benchmarks
{
	[CoreJob]
	[RPlotExporter]
	public class ComparisonBenchmarks
	{
		[Params ("1 + (10 / 40) > 1 == true", "1+1", "true == false")]
		public string Expression { get; set; }

		private CompiledExpression EvaluateTarget;
		private ExpressionSyntax CompileSyntax;
		private ExpressionEvaluator ExpressionEvaluatorInstance;

		[GlobalSetup]
		public void Setup ()
		{
			EvaluateTarget = CompiledExpression.Compile (Expression);
			CompileSyntax = new ExpressionSyntax (Expression);
			ExpressionEvaluatorInstance = new ExpressionEvaluator ();
		}

		[Benchmark]
		public void Parse () => new ExpressionSyntax (Expression);

		[Benchmark]
		public void Compile () => CompiledExpression.Compile (CompileSyntax);

		[Benchmark]
		public void Evaluate () => EvaluateTarget.Evaluate ();

		[Benchmark]
		public void ExpressionEvaluator () => ExpressionEvaluatorInstance.Evaluate (Expression);
	}
}
