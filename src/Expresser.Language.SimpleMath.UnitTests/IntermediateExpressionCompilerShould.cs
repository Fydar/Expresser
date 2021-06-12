using Expresser.Language.SimpleMath.Runtime;
using NUnit.Framework;

namespace Expresser.Language.SimpleMath.UnitTests
{
	public class IntermediateExpressionCompilerShould
	{
		[Test]
		public void CompileBasicMaths()
		{
			string syntax = "1 + 1 / 20";
			var intermediateExpression = SimpleMathExpression.Compile(syntax);


		}

		[Test]
		public void CompileComplexLogicMaths()
		{
			string syntax = "1 + (10 / 40) > 1 == true";
			var intermediateExpression = SimpleMathExpression.Compile(syntax);

		}

		[Test]
		public void CompileBasicLogic()
		{
			string syntax = "10 > 5 == true";
			var intermediateExpression = SimpleMathExpression.Compile(syntax);


		}

		[Test]
		public void CompileBranchingExpression()
		{
			string syntax = "10 * 10 + 2 * 2";
			var intermediateExpression = SimpleMathExpression.Compile(syntax);


		}

		[Test]
		public void ImportValues()
		{
			var context = new MathContextBuilder()
				.WithTerm("Width", new StaticValueProvider(10))
				.Build();

			string syntax = "10 * Width";
			var intermediateExpression = SimpleMathExpression.Compile(syntax, context);


		}

		[Test]
		public void OrderFromParanthesis()
		{
			string syntax = "(10 + 10) ^ 2";
			var intermediateExpression = SimpleMathExpression.Compile(syntax);


		}

		[SetUp]
		public void Setup()
		{

		}
	}
}
