using Expresser.Processing;
using System.Text;

namespace Expresser
{
	/// <summary>
	/// <para>A mathematical expression in a compiled format.</para>
	/// </summary>
	public class CompiledExpression
	{
		private readonly MathValue[] CalculationBuffer;

		/// <summary>
		/// <para></para>
		/// </summary>
		public IMathContext Context { get; private set; }

		/// <summary>
		/// <para></para>
		/// </summary>
		public IntermediateExpression Intermediate { get; private set; }

		/// <summary>
		/// <para></para>
		/// </summary>
		public ExpressionSyntax Syntax { get; private set; }

		CompiledExpression (ExpressionSyntax syntax, IMathContext context = null)
		{
			Syntax = syntax;
			Context = context;

			Intermediate = IntermediateExpression.Compile (syntax, context);
			CalculationBuffer = new MathValue[Intermediate.DistSize];
		}

		public static CompiledExpression Compile (ExpressionSyntax syntax, IMathContext context = null)
		{
			return new CompiledExpression (syntax, context);
		}

		public static CompiledExpression Compile (string expression, IMathContext context = null)
		{
			return new CompiledExpression (new ExpressionSyntax(expression), context);
		}

		public MathValue Evaluate ()
		{
			return Intermediate.Evaluate (CalculationBuffer);
		}

		public override string ToString ()
		{
			var sb = new StringBuilder ();

			var lastToken = Syntax.Tokens[0];
			for (int i = 1; i < Syntax.Tokens.Count; i++)
			{
				var token = Syntax.Tokens[i];
				sb.Append (lastToken);
				if (lastToken.Operation != SyntaxTokenKind.OpenParentheses
					&& token.Operation != SyntaxTokenKind.CloseParentheses
					&& token.Operation != SyntaxTokenKind.Percentage
					&& token.Operation != SyntaxTokenKind.Comma
					&& token.Operation != SyntaxTokenKind.Not)
				{
					sb.Append (' ');
				}
				lastToken = token;
			}
			sb.Append (Syntax.Tokens[Syntax.Tokens.Count - 1]);
			return sb.ToString ();
		}
	}
}
