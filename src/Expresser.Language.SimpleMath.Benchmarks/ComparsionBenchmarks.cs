using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using CodingSeb.ExpressionEvaluator;

namespace Expresser.Benchmarks
{
	[RPlotExporter]
	[SimpleJob(RuntimeMoniker.NetCoreApp22)]
	public class ComparisonBenchmarks
	{
		[Params("1 + (10 / 40) > 1 == true", "1+1", "true == false")]
		public string Expression { get; set; } = string.Empty;

		// Expresser
		private SimpleMathExpression? evaluateTarget;

		// CodingSeb.ExpressionEvaluator
		private ExpressionEvaluator? expressionEvaluatorInstance;

		[GlobalSetup]
		public void Setup()
		{
			evaluateTarget = SimpleMathExpression.Compile(Expression);
			expressionEvaluatorInstance = new ExpressionEvaluator();
		}

		[Benchmark]
		public void Compile() => SimpleMathExpression.Compile(Expression);

		[Benchmark]
		public void Evaluate() => evaluateTarget!.Evaluate();

		[Benchmark]
		public void ExpressionEvaluator() => expressionEvaluatorInstance!.Evaluate(Expression);
	}
}
