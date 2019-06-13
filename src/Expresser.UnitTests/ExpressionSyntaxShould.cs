using Expresser;
using NUnit.Framework;

namespace Tests
{
	public class ExpressionSyntaxShould
	{
		[Test]
		public void HandleCommaDelitedParameters ()
		{
			var syntax = new ExpressionSyntax ("1, 4");

			Assert.AreEqual (syntax.Tokens, new ExpressionToken[] {
				ExpressionToken.StaticValue(1),
				ExpressionToken.Operator(SyntaxTokenKind.Comma),
				ExpressionToken.StaticValue(4)
			});
		}

		[Test]
		public void HandleDoubleParenthasis ()
		{
			var syntax = new ExpressionSyntax ("((1 + 1) + 1)");

			Assert.AreEqual (syntax.Tokens, new ExpressionToken[] {
				ExpressionToken.Operator(SyntaxTokenKind.OpenParentheses),
				ExpressionToken.Operator(SyntaxTokenKind.OpenParentheses),
				ExpressionToken.StaticValue(1),
				ExpressionToken.Operator(SyntaxTokenKind.Plus),
				ExpressionToken.StaticValue(1),
				ExpressionToken.Operator(SyntaxTokenKind.CloseParentheses),
				ExpressionToken.Operator(SyntaxTokenKind.Plus),
				ExpressionToken.StaticValue(1),
				ExpressionToken.Operator(SyntaxTokenKind.CloseParentheses),
			});
		}
	}
}
