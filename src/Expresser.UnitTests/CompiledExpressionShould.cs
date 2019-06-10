using Expresser;
using Expresser.Processing;
using NUnit.Framework;

namespace Tests
{
	public class CompiledExpressionShould
	{
		[SetUp]
		public void Setup ()
		{

		}

		[Test]
		public void ImportTerms ()
		{
			var contextA = new MathContextBuilder ()
				.WithTerm ("Width", new StaticValueProvider (15))
				.WithTerm ("Height", new StaticValueProvider (10))
				.Build ();

			TestExpression (-100, IntermediateExpression.Compile (new ExpressionSyntax ("10 * -Height"), contextA));
			TestExpression (25, IntermediateExpression.Compile (new ExpressionSyntax ("width + Height"), contextA));
			TestExpression (25, IntermediateExpression.Compile (new ExpressionSyntax ("wiDth + heiGHt"), contextA));

			TestExpression (20, IntermediateExpression.Compile (new ExpressionSyntax ("Height * 2"), contextA));
			TestExpression (23, IntermediateExpression.Compile (new ExpressionSyntax ("3 + Height * 2"), contextA));

			TestExpression (5, IntermediateExpression.Compile (new ExpressionSyntax ("Width + -Height"), contextA));
		}

		[Test]
		public void LogicOnTerms ()
		{
			var contextA = new MathContextBuilder ()
				.WithTerm ("Level", new StaticValueProvider (15))
				.Build ();

			TestExpression (true, IntermediateExpression.Compile (new ExpressionSyntax ("2 < Level"), contextA));
			TestExpression (true, IntermediateExpression.Compile (new ExpressionSyntax ("15 == Level"), contextA));
			TestExpression (true, IntermediateExpression.Compile (new ExpressionSyntax ("16 > Level"), contextA));
		}

		[Test]
		public void OperateComparisons ()
		{
			TestExpression (true, IntermediateExpression.Compile (new ExpressionSyntax ("2 > 1")));
			TestExpression (false, IntermediateExpression.Compile (new ExpressionSyntax ("1 > 1")));
			TestExpression (false, IntermediateExpression.Compile (new ExpressionSyntax ("0 > 1")));

			TestExpression (true, IntermediateExpression.Compile (new ExpressionSyntax ("1 >= 1")));

			TestExpression (false, IntermediateExpression.Compile (new ExpressionSyntax ("2 <= 1")));

			TestExpression (true, IntermediateExpression.Compile (new ExpressionSyntax ("true || 2 < 1")));
		}

		[Test]
		public void OperateEquality ()
		{
			// Equality Operators
			TestExpression (true, IntermediateExpression.Compile (new ExpressionSyntax ("1 == 1")));
			TestExpression (false, IntermediateExpression.Compile (new ExpressionSyntax ("1 != 1")));
			TestExpression (false, IntermediateExpression.Compile (new ExpressionSyntax ("1 == 10")));

			TestExpression (true, IntermediateExpression.Compile (new ExpressionSyntax ("true == true")));
			TestExpression (false, IntermediateExpression.Compile (new ExpressionSyntax ("true != true")));

			TestExpression (true, IntermediateExpression.Compile (new ExpressionSyntax ("false == false")));
			TestExpression (false, IntermediateExpression.Compile (new ExpressionSyntax ("false != false")));
		}

		[Test]
		public void OperateLogic ()
		{
			// Logic Operators
			TestExpression (true, IntermediateExpression.Compile (new ExpressionSyntax ("true && true")));
			TestExpression (false, IntermediateExpression.Compile (new ExpressionSyntax ("true && false")));
			TestExpression (false, IntermediateExpression.Compile (new ExpressionSyntax ("false && true")));
			TestExpression (false, IntermediateExpression.Compile (new ExpressionSyntax ("false && false")));

			TestExpression (true, IntermediateExpression.Compile (new ExpressionSyntax ("true || true")));
			TestExpression (true, IntermediateExpression.Compile (new ExpressionSyntax ("false || true")));
			TestExpression (true, IntermediateExpression.Compile (new ExpressionSyntax ("true || false")));
			TestExpression (false, IntermediateExpression.Compile (new ExpressionSyntax ("false || false")));
		}

		[Test]
		public void OperateMaths ()
		{
			TestExpression (2, IntermediateExpression.Compile (new ExpressionSyntax ("3 + -1")));
			TestExpression (2, IntermediateExpression.Compile (new ExpressionSyntax ("1     + (1)")));

			// Add Operator
			TestExpression (2, IntermediateExpression.Compile (new ExpressionSyntax ("1 + 1")));
			TestExpression (2, IntermediateExpression.Compile (new ExpressionSyntax ("1     + 1")));
			TestExpression (5, IntermediateExpression.Compile (new ExpressionSyntax ("((1 + 3) + 1)")));

			TestExpression (25, IntermediateExpression.Compile (new ExpressionSyntax ("10+15")));
			TestExpression (20, IntermediateExpression.Compile (new ExpressionSyntax ("10 + 10")));
			TestExpression (25, IntermediateExpression.Compile (new ExpressionSyntax ("10 + 10 + 5")));

			// Negative Numbers
			TestExpression (2, IntermediateExpression.Compile (new ExpressionSyntax ("3 + -1")));
			TestExpression (3, IntermediateExpression.Compile (new ExpressionSyntax ("-1 + 4")));

			// Subtract Operator
			TestExpression (2, IntermediateExpression.Compile (new ExpressionSyntax ("3 - 1")));

			// Multiply Operator
			TestExpression (5, IntermediateExpression.Compile (new ExpressionSyntax ("1 * 5")));
			TestExpression (20, IntermediateExpression.Compile (new ExpressionSyntax ("200 * 0.1")));

			// Divide Operator
			TestExpression (5, IntermediateExpression.Compile (new ExpressionSyntax ("10 / 2")));

			// Power Operator
			TestExpression (27, IntermediateExpression.Compile (new ExpressionSyntax ("3 ^ 3")));
			TestExpression (27, IntermediateExpression.Compile (new ExpressionSyntax ("3 ^ (1 + 2)")));

			// Order of Operations Tests
			TestExpression (-5, IntermediateExpression.Compile (new ExpressionSyntax ("10 - 10 + 5")));
			TestExpression (-5, IntermediateExpression.Compile (new ExpressionSyntax ("10 -(10+   5)")));
			TestExpression (5, IntermediateExpression.Compile (new ExpressionSyntax ("(10 - 10) + 5")));

			TestExpression (12.564f, IntermediateExpression.Compile (new ExpressionSyntax ("3.141 * 2^2")));
		}

		[Test]
		public void OperatePercentages ()
		{
			var contextA = new MathContextBuilder ()
				.WithTerm ("Width", new StaticValueProvider (15))
				.WithTerm ("Height", new StaticValueProvider (10))
				.Build ();

			TestExpression (5, IntermediateExpression.Compile (new ExpressionSyntax ("50% * Height"), contextA));
			// TestExpression(5, new CompiledExpression("50% Height"), contextA));
		}

		public void TestExpression (float expected, CompiledExpression expression)
		{
			TestContext.Error.WriteLine (expression.ToString () + " = " + expected);
			Assert.AreEqual (expected, expression.Evaluate ().FloatValue);
		}

		public void TestExpression (bool expected, CompiledExpression expression)
		{
			TestContext.Error.WriteLine (expression.ToString () + " = " + expected);
			Assert.AreEqual (expected, expression.Evaluate ().BoolValue);
		}

		public void TestExpression (float expected, IntermediateExpression expression)
		{
			TestContext.Error.WriteLine (expression.ToString () + " = " + expected);
			Assert.AreEqual (expected, expression.Evaluate ().FloatValue);
		}

		public void TestExpression (bool expected, IntermediateExpression expression)
		{
			TestContext.Error.WriteLine (expression.ToString () + " = " + expected);
			Assert.AreEqual (expected, expression.Evaluate ().BoolValue);
		}

		[Test]
		public void TokeniseString ()
		{
			TestContext.Error.WriteLine (IntermediateExpression.Compile (new ExpressionSyntax ("1 + 1")));
			TestContext.Error.WriteLine (IntermediateExpression.Compile (new ExpressionSyntax ("2 > 1")));
			TestContext.Error.WriteLine (IntermediateExpression.Compile (new ExpressionSyntax ("2 > 1 && 4 > 1")));
		}
	}
}
