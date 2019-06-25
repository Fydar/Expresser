using Expresser.Input;
using Expresser.Processing;
using System.Text;

namespace Expresser
{
	/// <summary>
	/// <para>A mathematical expression in a compiled format.</para>
	/// </summary>
	/// <example>
	/// <para>Below is an example of constructing a CompiledExpression with a string and evaluating it.</para>
	/// <code>
	/// using System;
	/// using Expresser;
	/// 
	/// public class Program
	/// {
	/// 	public static void Main (string[] args)
	/// 	{
	/// 		var context = new MathContextBuilder ()
	/// 			.WithTerm ("Width", new StaticValueProvider (10))
	/// 			.Build ();
	/// 
	/// 		var expression = new CompiledExpression ("0.1*Width", context);
	/// 
	/// 		var result = expression.Evaluate ();
	/// 
	/// 		Console.WriteLine (expression);        // 0.1 * Width
	/// 		Console.WriteLine (result.ValueClass); // ValueClassifier.Float
	/// 		Console.WriteLine (result.FloatValue); // 1
	/// 	}
	/// }
	/// </code>
	/// </example>
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
			return new CompiledExpression (new ExpressionSyntax (expression), context);
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
					&& lastToken.Operation != SyntaxTokenKind.Not)
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
