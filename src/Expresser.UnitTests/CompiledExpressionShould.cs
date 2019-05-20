using Expresser;
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

			TestExpression (-100, new CompiledExpression ("10 * -Height", contextA));
			TestExpression (25, new CompiledExpression ("width + Height", contextA));
			TestExpression (25, new CompiledExpression ("wiDth + heiGHt", contextA));

			TestExpression (20, new CompiledExpression ("Height * 2", contextA));
			TestExpression (23, new CompiledExpression ("3 + Height * 2", contextA));

			TestExpression (5, new CompiledExpression ("Width + -Height", contextA));
		}

		[Test]
		public void LogicOnTerms ()
		{
			var contextA = new MathContextBuilder ()
				.WithTerm ("Level", new StaticValueProvider (15))
				.Build ();

			TestExpression (true, new CompiledExpression ("2 < Level", contextA));
			TestExpression (true, new CompiledExpression ("15 == Level", contextA));
			TestExpression (true, new CompiledExpression ("16 > Level", contextA));
		}

		[Test]
		public void OperateComparisons ()
		{
			TestExpression (true, new CompiledExpression ("2 > 1"));
			TestExpression (false, new CompiledExpression ("1 > 1"));
			TestExpression (false, new CompiledExpression ("0 > 1"));

			TestExpression (true, new CompiledExpression ("1 >= 1"));

			TestExpression (false, new CompiledExpression ("2 <= 1"));

			TestExpression (true, new CompiledExpression ("true || 2 < 1"));
		}

		[Test]
		public void OperateEquality ()
		{
			// Equality Operators
			TestExpression (true, new CompiledExpression ("1 == 1"));
			TestExpression (false, new CompiledExpression ("1 != 1"));
			TestExpression (false, new CompiledExpression ("1 == 10"));

			TestExpression (true, new CompiledExpression ("true == true"));
			TestExpression (false, new CompiledExpression ("true != true"));

			TestExpression (true, new CompiledExpression ("false == false"));
			TestExpression (false, new CompiledExpression ("false != false"));
		}

		[Test]
		public void OperateLogic ()
		{
			// Logic Operators
			TestExpression (true, new CompiledExpression ("true && true"));
			TestExpression (false, new CompiledExpression ("true && false"));
			TestExpression (false, new CompiledExpression ("false && true"));
			TestExpression (false, new CompiledExpression ("false && false"));

			TestExpression (true, new CompiledExpression ("true || true"));
			TestExpression (true, new CompiledExpression ("false || true"));
			TestExpression (true, new CompiledExpression ("true || false"));
			TestExpression (false, new CompiledExpression ("false || false"));
		}

		[Test]
		public void OperateMaths ()
		{
			TestExpression (2, new CompiledExpression ("3 + -1"));
			TestExpression (2, new CompiledExpression ("1     + (1)"));

			// Add Operator
			TestExpression (2, new CompiledExpression ("1 + 1"));
			TestExpression (2, new CompiledExpression ("1     + 1"));
			TestExpression (5, new CompiledExpression ("((1 + 3) + 1)"));

			TestExpression (25, new CompiledExpression ("10+15"));
			TestExpression (20, new CompiledExpression ("10 + 10"));
			TestExpression (25, new CompiledExpression ("10 + 10 + 5"));

			// Negative Numbers
			TestExpression (2, new CompiledExpression ("3 + -1"));
			TestExpression (3, new CompiledExpression ("-1 + 4"));

			// Subtract Operator
			TestExpression (2, new CompiledExpression ("3 - 1"));

			// Multiply Operator
			TestExpression (5, new CompiledExpression ("1 * 5"));
			TestExpression (20, new CompiledExpression ("200 * 0.1"));

			// Divide Operator
			TestExpression (5, new CompiledExpression ("10 / 2"));

			// Power Operator
			TestExpression (27, new CompiledExpression ("3 ^ 3"));
			TestExpression (27, new CompiledExpression ("3 ^ (1 + 2)"));

			// Order of Operations Tests
			TestExpression (-5, new CompiledExpression ("10 - 10 + 5"));
			TestExpression (-5, new CompiledExpression ("10 -(10+   5)"));
			TestExpression (5, new CompiledExpression ("(10 - 10) + 5"));

			TestExpression (12.564f, new CompiledExpression ("3.141 * 2^2"));
		}

		[Test]
		public void OperatePercentages ()
		{
			var contextA = new MathContextBuilder ()
				.WithTerm ("Width", new StaticValueProvider (15))
				.WithTerm ("Height", new StaticValueProvider (10))
				.Build ();

			TestExpression (5, new CompiledExpression ("50% * Height", contextA));
			// TestExpression(5, new CompiledExpression("50% Height", contextA));
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
			TestContext.Error.WriteLine (new CompiledExpression ("1 + 1"));
			TestContext.Error.WriteLine (new CompiledExpression ("2 > 1"));
			TestContext.Error.WriteLine (new CompiledExpression ("2 > 1 && 4 > 1"));
		}
	}
}
