using System;

namespace Expresser.Processing
{
	public struct IntermediateOperationActions
	{
		private IMathContext Context { get; }

		public IntermediateOperationActions (IMathContext context)
		{
			Context = context;
		}

		private MathValue ResolveNumericType (MathValue source)
		{
			if (source.ValueClass == ValueClassifier.FloatFractional)
			{
				if (Context.ImplicitReference != null)
				{
					return new MathValue (Context.ImplicitReference.Value.FloatValue * source.FloatValue, false);
				}
				else
				{
					throw new InvalidOperationException ("Could not resolve \"" + source + "\" to a numeric type");
				}
			}
			if (source.ValueClass == ValueClassifier.Float)
			{
				return source;
			}
			throw new InvalidOperationException ("Could not resolve \"" + source + "\" to a numeric type");
		}

		public MathValue Power (MathValue x, MathValue y)
		{
			if (x.ValueClass != y.ValueClass || x.ValueClass != ValueClassifier.Float)
			{
				throw new InvalidOperationException (string.Format ("Cannot Power a \"{0}\" and a \"{1}\" together", x.ValueClass, y.ValueClass));
			}

			return new MathValue ((float)Math.Pow (x.FloatValue, y.FloatValue), false);
		}

		public MathValue Add (MathValue left, MathValue right)
		{
			left = ResolveNumericType (left);
			right = ResolveNumericType (right);
			if (left.ValueClass != right.ValueClass || left.ValueClass != ValueClassifier.Float)
			{
				throw new InvalidOperationException (string.Format ("Cannot add a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
			}

			return new MathValue (left.FloatValue + right.FloatValue, false);
		}

		public MathValue Subtract (MathValue left, MathValue right)
		{
			left = ResolveNumericType (left);
			right = ResolveNumericType (right);
			if (left.ValueClass != right.ValueClass || left.ValueClass != ValueClassifier.Float)
			{
				throw new InvalidOperationException (string.Format ("Cannot subtract a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
			}

			return new MathValue (left.FloatValue - right.FloatValue, false);
		}

		public MathValue Multiply (MathValue left, MathValue right)
		{
			left = ResolveNumericType (left);
			right = ResolveNumericType (right);
			if (left.ValueClass != right.ValueClass || left.ValueClass != ValueClassifier.Float)
			{
				throw new InvalidOperationException (string.Format ("Cannot multiply a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
			}

			return new MathValue (left.FloatValue * right.FloatValue, false);
		}

		public MathValue Divide (MathValue left, MathValue right)
		{
			left = ResolveNumericType (left);
			right = ResolveNumericType (right);
			if (left.ValueClass != right.ValueClass || left.ValueClass != ValueClassifier.Float)
			{
				throw new InvalidOperationException (string.Format ("Cannot divide a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
			}

			return new MathValue (left.FloatValue / right.FloatValue, false);
		}

		public MathValue GreaterThan (MathValue left, MathValue right)
		{
			left = ResolveNumericType (left);
			right = ResolveNumericType (right);
			if (left.ValueClass != right.ValueClass || left.ValueClass != ValueClassifier.Float)
			{
				throw new InvalidOperationException (string.Format ("Cannot divide a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
			}

			return new MathValue (left.FloatValue > right.FloatValue);
		}

		public MathValue LessThan (MathValue left, MathValue right)
		{
			left = ResolveNumericType (left);
			right = ResolveNumericType (right);
			if (left.ValueClass != right.ValueClass || left.ValueClass != ValueClassifier.Float)
			{
				throw new InvalidOperationException (string.Format ("Cannot divide a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
			}

			return new MathValue (left.FloatValue < right.FloatValue);
		}

		public MathValue GreaterThanOrEqual (MathValue left, MathValue right)
		{
			left = ResolveNumericType (left);
			right = ResolveNumericType (right);
			if (left.ValueClass != right.ValueClass || left.ValueClass != ValueClassifier.Float)
			{
				throw new InvalidOperationException (string.Format ("Cannot divide a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
			}

			return new MathValue (left.FloatValue >= right.FloatValue);
		}

		public MathValue LessThanOrEqual (MathValue left, MathValue right)
		{
			left = ResolveNumericType (left);
			right = ResolveNumericType (right);
			if (left.ValueClass != right.ValueClass || left.ValueClass != ValueClassifier.Float)
			{
				throw new InvalidOperationException (string.Format ("Cannot divide a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
			}

			return new MathValue (left.FloatValue <= right.FloatValue);
		}

		public MathValue And (MathValue left, MathValue right)
		{
			if (left.ValueClass != right.ValueClass || left.ValueClass != ValueClassifier.Boolean)
			{
				throw new InvalidOperationException (string.Format ("Cannot AND a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
			}

			return new MathValue (left.BoolValue && right.BoolValue);
		}

		public MathValue Or (MathValue left, MathValue right)
		{
			if (left.ValueClass != right.ValueClass || left.ValueClass != ValueClassifier.Boolean)
			{
				throw new InvalidOperationException (string.Format ("Cannot AND a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
			}

			return new MathValue (left.BoolValue || right.BoolValue);
		}

		public MathValue Not (MathValue right)
		{
			if (right.ValueClass != ValueClassifier.Boolean)
			{
				throw new InvalidOperationException (string.Format ("Cannot perform NOT operator on \"{0}\"",  right.ValueClass));
			}

			return new MathValue (!right.BoolValue);
		}

		public MathValue Equal (MathValue left, MathValue right)
		{
			switch (left.ValueClass)
			{
				case ValueClassifier.FloatFractional:
				case ValueClassifier.Float:
					left = ResolveNumericType (left);
					right = ResolveNumericType (right);
					return new MathValue (left.FloatValue == right.FloatValue);

				case ValueClassifier.Boolean:
					return new MathValue (left.FloatValue == right.FloatValue);
			}

			throw new InvalidOperationException (string.Format ("Cannot equality a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
		}

		public MathValue NotEqual (MathValue left, MathValue right)
		{
			switch (left.ValueClass)
			{
				case ValueClassifier.FloatFractional:
				case ValueClassifier.Float:
					left = ResolveNumericType (left);
					right = ResolveNumericType (right);
					return new MathValue (left.FloatValue != right.FloatValue);

				case ValueClassifier.Boolean:
					return new MathValue (left.FloatValue != right.FloatValue);
			}

			throw new InvalidOperationException (string.Format ("Cannot equality a \"{0}\" and a \"{1}\" together", left.ValueClass, right.ValueClass));
		}

		public MathValue Negate (MathValue value)
		{
			if (value.ValueClass == ValueClassifier.Float
				|| value.ValueClass == ValueClassifier.FloatFractional)
			{
				return new MathValue (-value.FloatValue, value.ValueClass == ValueClassifier.FloatFractional);
			}
			throw new InvalidOperationException (string.Format ("Cannot negative type {0}", value.ValueClass));
		}
	}
}
