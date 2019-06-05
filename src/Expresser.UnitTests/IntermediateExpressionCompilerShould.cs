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

		[Test]
		public void CompileBranchingExpression ()
		{
			var syntax = new ExpressionSyntax ("10 * 10 + 2 * 2");
			var intermediateExpression = IntermediateExpression.Compile (syntax);


		}

		[Test]
		public void OrderFromParanthesis ()
		{
			var syntax = new ExpressionSyntax ("(10 + 10) ^ 2");
			var intermediateExpression = IntermediateExpression.Compile (syntax);


		}
	}
}
