using BenchmarkDotNet.Attributes;

namespace Expresser.Benchmarks
{
	[CoreJob]
	[RPlotExporter]
	public class ComparedWithRaw
	{
		[Params("1 + (10 / 40) > 1 == true")]
		public string Expression { get; set; }

		private CompiledExpression evaluateTarget;
		private ExpressionSyntax compileSyntax;

		private bool result;

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

		[Benchmark]
		public void CSharp() => result = 1 + (10 / 40) > 1 == true;
	}
}
