using BenchmarkDotNet.Attributes;

namespace Expresser.Benchmarks
{
	[CoreJob]
	[RPlotExporter]
	public class BasicBenchmarks
	{
		[Params("1 + (10 / 40) > 1 == true", "1+1", "true == false")]
		public string Expression { get; set; }

		private CompiledExpression evaluateTarget;
		private ExpressionSyntax compileSyntax;

		[GlobalSetup]
		public void Setup()
		{
			evaluateTarget = CompiledExpression.Compile(Expression);
			compileSyntax = new ExpressionSyntax(Expression);
		}

		[Benchmark]
		public void Parse() => new ExpressionSyntax(Expression);

		[Benchmark]
		public void Compile() => CompiledExpression.Compile(compileSyntax);

		[Benchmark]
		public void Evaluate() => evaluateTarget.Evaluate();
	}
}
