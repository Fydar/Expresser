using System;
using System.Runtime.InteropServices;

namespace Expresser
{
	[StructLayout (LayoutKind.Explicit)]
	public struct MathValue
	{
		[FieldOffset (0)] public ValueClassifier ValueClass;
		[FieldOffset (1)] public float FloatValue;
		[FieldOffset (1)] public bool BoolValue;

		public MathValue (bool boolValue) : this ()
		{
			BoolValue = boolValue;
			ValueClass = ValueClassifier.Boolean;
		}

		public MathValue (float floatValue, bool isFractional) : this ()
		{
			FloatValue = floatValue;
			ValueClass = isFractional ? ValueClassifier.Fractional : ValueClassifier.Numeric;
		}

		public override string ToString ()
		{
			switch (ValueClass)
			{
				case ValueClassifier.Fractional:
					return (FloatValue / 100).ToString () + "%";

				case ValueClassifier.Numeric:
					return FloatValue.ToString ();

				case ValueClassifier.Boolean:
					return BoolValue.ToString ();

				default:
					return "null";
			}
		}

		public static MathValue Power (MathValue x, MathValue y)
		{
			if (x.ValueClass != y.ValueClass || x.ValueClass != ValueClassifier.Numeric)
			{
				throw new InvalidOperationException (string.Format ("Cannot Power a \"{0}\" and a \"{1}\" together", x.ValueClass, y.ValueClass));
			}

			return new MathValue ((float)Math.Pow (x.FloatValue, y.FloatValue), false);
		}

		public static MathValue Add (MathValue left, MathValue right)
		{
			if (left.ValueClass != right.ValueClass || left.ValueClass != ValueClassifier.Numeric)
			{
				throw new InvalidOperationException (string.Format ("Cannot add a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
			}

			return new MathValue (left.FloatValue + right.FloatValue, false);
		}

		public static MathValue Subtract (MathValue left, MathValue right)
		{
			if (left.ValueClass != right.ValueClass || left.ValueClass != ValueClassifier.Numeric)
			{
				throw new InvalidOperationException (string.Format ("Cannot subtract a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
			}

			return new MathValue (left.FloatValue - right.FloatValue, false);
		}

		public static MathValue Multiply (MathValue left, MathValue right)
		{
			if (left.ValueClass != right.ValueClass || left.ValueClass != ValueClassifier.Numeric)
			{
				throw new InvalidOperationException (string.Format ("Cannot multiply a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
			}

			return new MathValue (left.FloatValue * right.FloatValue, false);
		}

		public static MathValue Divide (MathValue left, MathValue right)
		{
			if (left.ValueClass != right.ValueClass || left.ValueClass != ValueClassifier.Numeric)
			{
				throw new InvalidOperationException (string.Format ("Cannot divide a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
			}

			return new MathValue (left.FloatValue / right.FloatValue, false);
		}

		public static MathValue GreaterThan (MathValue left, MathValue right)
		{
			if (left.ValueClass != right.ValueClass || left.ValueClass != ValueClassifier.Numeric)
			{
				throw new InvalidOperationException (string.Format ("Cannot divide a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
			}

			return new MathValue (left.FloatValue > right.FloatValue);
		}

		public static MathValue LessThan (MathValue left, MathValue right)
		{
			if (left.ValueClass != right.ValueClass || left.ValueClass != ValueClassifier.Numeric)
			{
				throw new InvalidOperationException (string.Format ("Cannot divide a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
			}

			return new MathValue (left.FloatValue < right.FloatValue);
		}

		public static MathValue GreaterThanOrEqual (MathValue left, MathValue right)
		{
			if (left.ValueClass != right.ValueClass || left.ValueClass != ValueClassifier.Numeric)
			{
				throw new InvalidOperationException (string.Format ("Cannot divide a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
			}

			return new MathValue (left.FloatValue >= right.FloatValue);
		}

		public static MathValue LessThanOrEqual (MathValue left, MathValue right)
		{
			if (left.ValueClass != right.ValueClass || left.ValueClass != ValueClassifier.Numeric)
			{
				throw new InvalidOperationException (string.Format ("Cannot divide a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
			}

			return new MathValue (left.FloatValue <= right.FloatValue);
		}

		public static MathValue And (MathValue left, MathValue right)
		{
			if (left.ValueClass != right.ValueClass || left.ValueClass != ValueClassifier.Boolean)
			{
				throw new InvalidOperationException (string.Format ("Cannot AND a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
			}

			return new MathValue (left.BoolValue && right.BoolValue);
		}

		public static MathValue Or (MathValue left, MathValue right)
		{
			if (left.ValueClass != right.ValueClass || left.ValueClass != ValueClassifier.Boolean)
			{
				throw new InvalidOperationException (string.Format ("Cannot AND a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
			}

			return new MathValue (left.BoolValue || right.BoolValue);
		}

		public static MathValue Equal (MathValue left, MathValue right)
		{
			switch (left.ValueClass)
			{
				case ValueClassifier.Fractional:
				case ValueClassifier.Numeric:
					return new MathValue (left.FloatValue == right.FloatValue);

				case ValueClassifier.Boolean:
					return new MathValue (left.FloatValue == right.FloatValue);
			}

			throw new InvalidOperationException (string.Format ("Cannot equality a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
		}

		public static MathValue NotEqual (MathValue left, MathValue right)
		{
			switch (left.ValueClass)
			{
				case ValueClassifier.Fractional:
				case ValueClassifier.Numeric:
					return new MathValue (left.FloatValue != right.FloatValue);

				case ValueClassifier.Boolean:
					return new MathValue (left.FloatValue != right.FloatValue);
			}

			throw new InvalidOperationException (string.Format ("Cannot equality a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
		}
	}
}
