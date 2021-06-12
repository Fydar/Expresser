using Expresser.Language.SimpleMath.Lexing;
using Expresser.Language.SimpleMath.Runtime;
using Expresser.Lexing;
using NUnit.Framework;
using System;

namespace Expresser.Language.SimpleMath.UnitTests
{
	public class SimpleMathExpressionShould
	{
		[SetUp]
		public void Setup()
		{

		}

		[Test]
		public void ImportTerms()
		{
			var contextA = new MathContextBuilder()
				.WithTerm("Width", 15)
				.WithTerm("Height", 10)
				.Build();

			TestExpression(100, SimpleMathExpression.Compile("10 * Height", contextA));
			TestExpression(25, SimpleMathExpression.Compile("width + Height", contextA));
			TestExpression(25, SimpleMathExpression.Compile("wiDth + heiGHt", contextA));

			TestExpression(20, SimpleMathExpression.Compile("Height * 2", contextA));
			TestExpression(23, SimpleMathExpression.Compile("3 + Height * 2", contextA));
		}

		[Test]
		public void LogicOnTerms()
		{
			var contextA = new MathContextBuilder()
				.WithTerm("Level", new StaticValueProvider(15))
				.Build();

			TestExpression(true, SimpleMathExpression.Compile("2 < Level", contextA));
			TestExpression(true, SimpleMathExpression.Compile("15 == Level", contextA));
			TestExpression(true, SimpleMathExpression.Compile("16 > Level", contextA));
		}

		[Test]
		public void OperateComparisons()
		{
			TestExpression(true, SimpleMathExpression.Compile("2 > 1"));
			TestExpression(false, SimpleMathExpression.Compile("1 > 1"));
			TestExpression(false, SimpleMathExpression.Compile("0 > 1"));

			TestExpression(true, SimpleMathExpression.Compile("1 >= 1"));

			TestExpression(false, SimpleMathExpression.Compile("2 <= 1"));

			TestExpression(true, SimpleMathExpression.Compile("true | 2 < 1"));
		}

		[Test]
		public void OperateEquality()
		{
			// Equality Operators
			TestExpression(true, SimpleMathExpression.Compile("1 == 1"));
			TestExpression(false, SimpleMathExpression.Compile("1 != 1"));
			TestExpression(false, SimpleMathExpression.Compile("1 == 10"));

			TestExpression(true, SimpleMathExpression.Compile("true == true"));
			TestExpression(false, SimpleMathExpression.Compile("true != true"));

			TestExpression(true, SimpleMathExpression.Compile("false == false"));
			TestExpression(false, SimpleMathExpression.Compile("false != false"));
		}

		[Test]
		public void OperateLogic()
		{
			TestExpression(true, SimpleMathExpression.Compile("true"));
			TestExpression(false, SimpleMathExpression.Compile("false"));

			// AND Operator
			TestExpression(true, SimpleMathExpression.Compile("true & true"));
			TestExpression(false, SimpleMathExpression.Compile("true & false"));
			TestExpression(false, SimpleMathExpression.Compile("false & true"));
			TestExpression(false, SimpleMathExpression.Compile("false & false"));

			// OR Operator
			TestExpression(true, SimpleMathExpression.Compile("true | true"));
			TestExpression(true, SimpleMathExpression.Compile("false | true"));
			TestExpression(true, SimpleMathExpression.Compile("true | false"));
			TestExpression(false, SimpleMathExpression.Compile("false | false"));

			// NOT Operator
			TestExpression(false, SimpleMathExpression.Compile("!true"));
			TestExpression(true, SimpleMathExpression.Compile("!false"));
			TestExpression(true, SimpleMathExpression.Compile("!(false)"));

			TestExpression(false, SimpleMathExpression.Compile("!(30 > 10)"));
		}

		[Test]
		public void OperateMaths()
		{
			TestExpression(2, SimpleMathExpression.Compile("3 + -1"));
			TestExpression(2, SimpleMathExpression.Compile("1     + (1)"));

			// Add Operator
			TestExpression(2, SimpleMathExpression.Compile("1 + 1"));
			TestExpression(2, SimpleMathExpression.Compile("1     + 1"));
			TestExpression(5, SimpleMathExpression.Compile("((1 + 3) + 1)"));

			TestExpression(25, SimpleMathExpression.Compile("10+15"));
			TestExpression(20, SimpleMathExpression.Compile("10 + 10"));
			TestExpression(25, SimpleMathExpression.Compile("10 + 10 + 5"));

			// Negative Numbers
			TestExpression(2, SimpleMathExpression.Compile("3 + -1"));
			TestExpression(3, SimpleMathExpression.Compile("-1 + 4"));

			// Subtract Operator
			TestExpression(2, SimpleMathExpression.Compile("3 - 1"));

			// Multiply Operator
			TestExpression(5, SimpleMathExpression.Compile("1 * 5"));
			TestExpression(20, SimpleMathExpression.Compile("200 * 0.1"));

			// Divide Operator
			TestExpression(5, SimpleMathExpression.Compile("10 / 2"));

			// Power Operator
			TestExpression(27, SimpleMathExpression.Compile("3 ^ 3"));
			TestExpression(27, SimpleMathExpression.Compile("3 ^ (1 + 2)"));

			// Order of Operations Tests
			TestExpression(-5, SimpleMathExpression.Compile("10 - 10 + 5"));
			TestExpression(-5, SimpleMathExpression.Compile("10 -(10+   5)"));
			TestExpression(5, SimpleMathExpression.Compile("(10 - 10) + 5"));

			TestExpression(12.564f, SimpleMathExpression.Compile("3.141 * 2^2"));
		}

		public void TestExpression(MathValue expected, SimpleMathExpression expression)
		{
			Console.WriteLine(expression.ToString() + " = " + expected);
			Describe(SimpleMathLexerLanguage.Lexer, expression.ToString());
			Assert.AreEqual(expected, expression.Evaluate());
		}

		private static void Describe(Lexer lexer, string source)
		{
			Console.ForegroundColor = ConsoleColor.Gray;
			int index = 0;
			foreach (var token in lexer.Tokenize(source))
			{
				string typeName = lexer.LexerLanguage.Classifiers[token.Classifier].GetType().Name
					.Replace("TokenClassifier", "")
					.PadRight(16);

				string rendered = source
					.Substring(token.StartIndex, token.Length)
					.Replace(Environment.NewLine, "\\n");

				Console.WriteLine($"ID:{index,4}  Type:{token.Classifier,4}  {typeName}\tSymbol: \"{rendered}\"");
				index++;
			}
			Console.Write("\n");
		}
	}
}
