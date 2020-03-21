using BenchmarkDotNet.Attributes;
using CodingSeb.ExpressionEvaluator;

namespace Expresser.Benchmarks
{
	[CoreJob]
	[RPlotExporter]
	public class ComparisonBenchmarks
	{
		[Params("1 + (10 / 40) > 1 == true", "1+1", "true == false")]
		public string Expression { get; set; }

		// Expresser
		private CompiledExpression evaluateTarget;
		private ExpressionSyntax compileSyntax;

		// CodingSeb.ExpressionEvaluator
		private ExpressionEvaluator expressionEvaluatorInstance;

		[GlobalSetup]
		public void Setup()
		{
			evaluateTarget = CompiledExpression.Compile(Expression);
			compileSyntax = new ExpressionSyntax(Expression);
			expressionEvaluatorInstance = new ExpressionEvaluator();
		}

		[Benchmark]
		public void Parse() => new ExpressionSyntax(Expression);

		[Benchmark]
		public void Compile() => CompiledExpression.Compile(compileSyntax);

		[Benchmark]
		public void Evaluate() => evaluateTarget.Evaluate();

		[Benchmark]
		public void ExpressionEvaluator() => expressionEvaluatorInstance.Evaluate(Expression);
	}
}
