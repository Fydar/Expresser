using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Expresser.Benchmarks
{
	[RPlotExporter]
	[SimpleJob(RuntimeMoniker.CoreRt22)]
	public class ComparedWithRaw
	{
		[Params("1 + (10 / 40) > 1 == true")]
		public string Expression { get; set; }

		private SimpleMathExpression evaluateTarget;

		private bool result;

		[GlobalSetup]
		public void Setup()
		{
			evaluateTarget = SimpleMathExpression.Compile(Expression);
		}

		[Benchmark]
		public void Compile() => SimpleMathExpression.Compile(Expression);

		[Benchmark]
		public void Evaluate() => evaluateTarget.Evaluate();

		[Benchmark]
		public void CSharp() => result = 1 + (10 / 40) > 1 == true;
	}
}
