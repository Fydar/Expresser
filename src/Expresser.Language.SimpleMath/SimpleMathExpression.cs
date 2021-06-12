using Expresser.Language.SimpleMath.Compilation;
using Expresser.Language.SimpleMath.Runtime;

namespace Expresser
{
	/// <summary>
	/// <para>A simple maths expression in a compiled format.</para>
	/// </summary>
	public class SimpleMathExpression
	{
		private readonly MathValue[] runtimeBuffer;

		/// <summary>
		/// <para>The underlying expression.</para>
		/// </summary>
		public string Expression { get; }

		/// <summary>
		/// <para></para>
		/// </summary>
		public IMathContext? Context { get; private set; }

		/// <summary>
		/// <para></para>
		/// </summary>
		public IntermediateExpression Intermediate { get; private set; }

		private SimpleMathExpression(string expression, IMathContext? context = null)
		{
			Expression = expression;
			Context = context;

			Intermediate = IntermediateCompiler.Compile(expression, context);
			runtimeBuffer = new MathValue[Intermediate.DistSize];
		}

		public static SimpleMathExpression Compile(string expression, IMathContext? context = null)
		{
			return new SimpleMathExpression(expression, context);
		}

		public MathValue Evaluate()
		{
			return Intermediate.Evaluate(runtimeBuffer);
		}

		public override string ToString()
		{
			return Expression;
		}
	}
}
