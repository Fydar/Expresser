using System.Runtime.InteropServices;

namespace Expresser
{
	[StructLayout (LayoutKind.Explicit)]
	public struct ExpressionToken
	{
		[FieldOffset (0)] public OperationCode Operation;
		[FieldOffset (1)] public MathValue Value;
		[FieldOffset (1)] public byte Source;
		[FieldOffset (2)] public sbyte Multiplier;

		public static ExpressionToken None => new ExpressionToken () { Operation = OperationCode.None };

		public MathValue ValueWithSources (IValueProvider[] sources)
		{
			if (Operation == OperationCode.Source)
			{
				var sourceValue = sources[Source].Value;
				switch (sourceValue.ValueClass)
				{
					case ValueClassifier.Numeric: return new MathValue (sourceValue.FloatValue * Multiplier, false);
					default: return sourceValue;
				}
			}
			return Value;
		}

		public bool IsOperator => Operation == OperationCode.Plus ||
					Operation == OperationCode.Minus ||
					Operation == OperationCode.Multiply ||
					Operation == OperationCode.Divide ||
					Operation == OperationCode.Power;

		public static ExpressionToken Operator (OperationCode operation)
		{
			return new ExpressionToken ()
			{
				Operation = operation
			};
		}

		public static ExpressionToken ReadSource (byte sourceId, bool negative)
		{
			return new ExpressionToken ()
			{
				Operation = OperationCode.Source,
				Source = sourceId,
				Multiplier = negative ? (sbyte)-1 : (sbyte)1
			};
		}

		public static ExpressionToken StaticValue (MathValue value)
		{
			return new ExpressionToken ()
			{
				Operation = OperationCode.Value,
				Value = value
			};
		}

		public override string ToString ()
		{
			switch (Operation)
			{
				case OperationCode.OpenParentheses: return "(";
				case OperationCode.CloseParentheses: return ")";

				case OperationCode.Plus: return "+";
				case OperationCode.Minus: return "-";
				case OperationCode.Multiply: return "*";
				case OperationCode.Divide: return "/";
				case OperationCode.Power: return "^";
				case OperationCode.Percentage: return "%";

				case OperationCode.And: return "&&";
				case OperationCode.Or: return "||";
				case OperationCode.Equal: return "==";
				case OperationCode.NotEqual: return "!=";
				case OperationCode.GreaterThan: return ">";
				case OperationCode.GreaterThanOrEqual: return ">=";
				case OperationCode.LessThan: return "<";
				case OperationCode.LessThanOrEqual: return "<=";

				case OperationCode.Value: return Value.ToString ();
				case OperationCode.Source: return "source[" + Source.ToString () + "]";
				default: return Operation.ToString ();
			}
		}
	}
}
