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
				.WithTerm ("Width", 15)
				.WithTerm ("Height", 10)
				.Build ();

			TestExpression (-100, CompiledExpression.Compile ("10 * -Height", contextA));
			TestExpression (25, CompiledExpression.Compile ("width + Height", contextA));
			TestExpression (25, CompiledExpression.Compile ("wiDth + heiGHt", contextA));

			TestExpression (20, CompiledExpression.Compile ("Height * 2", contextA));
			TestExpression (23, CompiledExpression.Compile ("3 + Height * 2", contextA));

			TestExpression (5, CompiledExpression.Compile ("Width + -Height", contextA));
		}

		[Test]
		public void LogicOnTerms ()
		{
			var contextA = new MathContextBuilder ()
				.WithTerm ("Level", new StaticValueProvider (15))
				.Build ();

			TestExpression (true, CompiledExpression.Compile ("2 < Level", contextA));
			TestExpression (true, CompiledExpression.Compile ("15 == Level", contextA));
			TestExpression (true, CompiledExpression.Compile ("16 > Level", contextA));
		}

		[Test]
		public void OperateComparisons ()
		{
			TestExpression (true, CompiledExpression.Compile ("2 > 1"));
			TestExpression (false, CompiledExpression.Compile ("1 > 1"));
			TestExpression (false, CompiledExpression.Compile ("0 > 1"));

			TestExpression (true, CompiledExpression.Compile ("1 >= 1"));

			TestExpression (false, CompiledExpression.Compile ("2 <= 1"));

			TestExpression (true, CompiledExpression.Compile ("true || 2 < 1"));
		}

		[Test]
		public void OperateEquality ()
		{
			// Equality Operators
			TestExpression (true, CompiledExpression.Compile ("1 == 1"));
			TestExpression (false, CompiledExpression.Compile ("1 != 1"));
			TestExpression (false, CompiledExpression.Compile ("1 == 10"));

			TestExpression (true, CompiledExpression.Compile ("true == true"));
			TestExpression (false, CompiledExpression.Compile ("true != true"));

			TestExpression (true, CompiledExpression.Compile ("false == false"));
			TestExpression (false, CompiledExpression.Compile ("false != false"));
		}

		[Test]
		public void OperateLogic ()
		{
			// Logic Operators
			TestExpression (true, CompiledExpression.Compile ("true && true"));
			TestExpression (false, CompiledExpression.Compile ("true && false"));
			TestExpression (false, CompiledExpression.Compile ("false && true"));
			TestExpression (false, CompiledExpression.Compile ("false && false"));

			TestExpression (true, CompiledExpression.Compile ("true || true"));
			TestExpression (true, CompiledExpression.Compile ("false || true"));
			TestExpression (true, CompiledExpression.Compile ("true || false"));
			TestExpression (false, CompiledExpression.Compile ("false || false"));
		}

		[Test]
		public void OperateMaths ()
		{
			TestExpression (2, CompiledExpression.Compile ("3 + -1"));
			TestExpression (2, CompiledExpression.Compile ("1     + (1)"));

			// Add Operator
			TestExpression (2, CompiledExpression.Compile ("1 + 1"));
			TestExpression (2, CompiledExpression.Compile ("1     + 1"));
			TestExpression (5, CompiledExpression.Compile ("((1 + 3) + 1)"));

			TestExpression (25, CompiledExpression.Compile ("10+15"));
			TestExpression (20, CompiledExpression.Compile ("10 + 10"));
			TestExpression (25, CompiledExpression.Compile ("10 + 10 + 5"));

			// Negative Numbers
			TestExpression (2, CompiledExpression.Compile ("3 + -1"));
			TestExpression (3, CompiledExpression.Compile ("-1 + 4"));

			// Subtract Operator
			TestExpression (2, CompiledExpression.Compile ("3 - 1"));

			// Multiply Operator
			TestExpression (5, CompiledExpression.Compile ("1 * 5"));
			TestExpression (20, CompiledExpression.Compile ("200 * 0.1"));

			// Divide Operator
			TestExpression (5, CompiledExpression.Compile ("10 / 2"));

			// Power Operator
			TestExpression (27, CompiledExpression.Compile ("3 ^ 3"));
			TestExpression (27, CompiledExpression.Compile ("3 ^ (1 + 2)"));

			// Order of Operations Tests
			TestExpression (-5, CompiledExpression.Compile ("10 - 10 + 5"));
			TestExpression (-5, CompiledExpression.Compile ("10 -(10+   5)"));
			TestExpression (5, CompiledExpression.Compile ("(10 - 10) + 5"));

			TestExpression (12.564f, CompiledExpression.Compile ("3.141 * 2^2"));
		}

		[Test]
		public void OperatePercentages ()
		{
			var contextA = new MathContextBuilder ()
				.WithTerm ("Width", new StaticValueProvider (15))
				.WithTerm ("Height", new StaticValueProvider (10))
				.Build ();

			TestExpression (5, CompiledExpression.Compile ("50% * Height", contextA));
			// TestExpression(5, new CompiledExpression("50% Height", contextA)));
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

		[Test]
		public void TokeniseString ()
		{
			TestContext.Error.WriteLine (CompiledExpression.Compile ("1 + 1"));
			TestContext.Error.WriteLine (CompiledExpression.Compile ("2 > 1"));
			TestContext.Error.WriteLine (CompiledExpression.Compile ("2 > 1 && 4 > 1"));
		}
	}
}
