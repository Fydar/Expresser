using Expresser.Input;
using System;
using System.Runtime.InteropServices;

namespace Expresser
{
	[StructLayout(LayoutKind.Explicit)]
	public struct ExpressionToken : IComparable<ExpressionToken>
	{
		[FieldOffset(0)] public SyntaxTokenKind Operation;
		[FieldOffset(1)] public MathValue Value;
		[FieldOffset(1)] public byte Source;
		[FieldOffset(2)] public sbyte Multiplier;

		public static ExpressionToken None => new ExpressionToken() { Operation = SyntaxTokenKind.None };

		public MathValue ValueWithSources(IValueProvider[] sources)
		{
			if (Operation == SyntaxTokenKind.Source)
			{
				var sourceValue = sources[Source].Value;
				switch (sourceValue.ValueClass)
				{
					case ValueClassifier.Float: return new MathValue(sourceValue.FloatValue * Multiplier, false);
					default: return sourceValue;
				}
			}
			return Value;
		}

		public bool IsOperator => Operation == SyntaxTokenKind.Plus ||
					Operation == SyntaxTokenKind.Minus ||
					Operation == SyntaxTokenKind.Multiply ||
					Operation == SyntaxTokenKind.Divide ||
					Operation == SyntaxTokenKind.Power;

		public static ExpressionToken Operator(SyntaxTokenKind operation)
		{
			return new ExpressionToken()
			{
				Operation = operation
			};
		}

		public static ExpressionToken ReadSource(byte sourceId, bool negative)
		{
			return new ExpressionToken()
			{
				Operation = SyntaxTokenKind.Source,
				Source = sourceId,
				Multiplier = negative ? (sbyte)-1 : (sbyte)1
			};
		}

		public static ExpressionToken StaticValue(MathValue value)
		{
			return new ExpressionToken()
			{
				Operation = SyntaxTokenKind.Value,
				Value = value
			};
		}

		public override string ToString()
		{
			switch (Operation)
			{
				case SyntaxTokenKind.OpenParentheses: return "(";
				case SyntaxTokenKind.CloseParentheses: return ")";
				case SyntaxTokenKind.Comma: return ",";

				case SyntaxTokenKind.Plus: return "+";
				case SyntaxTokenKind.Minus: return "-";
				case SyntaxTokenKind.Multiply: return "*";
				case SyntaxTokenKind.Divide: return "/";
				case SyntaxTokenKind.Power: return "^";
				case SyntaxTokenKind.Percentage: return "%";

				case SyntaxTokenKind.And: return "&&";
				case SyntaxTokenKind.Or: return "||";
				case SyntaxTokenKind.Not: return "!";
				case SyntaxTokenKind.Equal: return "==";
				case SyntaxTokenKind.NotEqual: return "!=";
				case SyntaxTokenKind.GreaterThan: return ">";
				case SyntaxTokenKind.GreaterThanOrEqual: return ">=";
				case SyntaxTokenKind.LessThan: return "<";
				case SyntaxTokenKind.LessThanOrEqual: return "<=";

				case SyntaxTokenKind.Value: return Value.ToString();
				case SyntaxTokenKind.Source: return "source[" + Source.ToString() + "]";
				default: return Operation.ToString();
			}
		}

		public int CompareTo(ExpressionToken other)
		{
			return Operation.CompareTo(other.Operation);
		}
	}
}
