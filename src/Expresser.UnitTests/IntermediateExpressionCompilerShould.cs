using Expresser;
using Expresser.Processing;
using NUnit.Framework;

namespace Tests
{
	public class IntermediateExpressionCompilerShould
	{
		[Test]
		public void CompileBasicMaths ()
		{
			var syntax = new ExpressionSyntax ("1 + 1 / 20");
			var intermediateExpression = IntermediateExpression.Compile (syntax);


		}
		[Test]
		public void CompileBasicLogic ()
		{
			var syntax = new ExpressionSyntax ("10 > 5 == true");
			var intermediateExpression = IntermediateExpression.Compile (syntax);


		}

		[Test]
		public void CompileBranchingExpression ()
		{
			var syntax = new ExpressionSyntax ("10 * 10 + 2 * 2");
			var intermediateExpression = IntermediateExpression.Compile (syntax);


		}

		[Test]
		public void ImportValues ()
		{
			var context = new MathContextBuilder ()
				.WithTerm ("Width", new StaticValueProvider (10))
				.Build ();

			var syntax = new ExpressionSyntax ("10 * Width");
			var intermediateExpression = IntermediateExpression.Compile (syntax, context);


		}

		[Test]
		public void OrderFromParanthesis ()
		{
			var syntax = new ExpressionSyntax ("(10 + 10) ^ 2");
			var intermediateExpression = IntermediateExpression.Compile (syntax);


		}

		[SetUp]
		public void Setup ()
		{

		}
	}
}
