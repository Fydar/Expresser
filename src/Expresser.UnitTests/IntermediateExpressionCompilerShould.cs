using Expresser;
using Expresser.Processing;
using NUnit.Framework;

namespace Tests
{
	public class IntermediateExpressionCompilerShould
	{
		[SetUp]
		public void Setup ()
		{

		}

		[Test]
		public void CompileBasicMaths ()
		{
			var syntax = new ExpressionSyntax ("1 + 1 / 20");
			var intermediateExpression = IntermediateExpression.Compile (syntax);


		}
	}
}
