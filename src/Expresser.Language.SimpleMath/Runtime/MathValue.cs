using System.Runtime.InteropServices;

namespace Expresser.Language.SimpleMath.Runtime
{
	[StructLayout(LayoutKind.Explicit)]
	public struct MathValue
	{
		[FieldOffset(0)] public ValueClassifier ValueClass;
		[FieldOffset(1)] public float FloatValue;
		[FieldOffset(1)] public float IntValue;
		[FieldOffset(1)] public bool BoolValue;

		public MathValue(bool boolValue) : this()
		{
			BoolValue = boolValue;
			ValueClass = ValueClassifier.Boolean;
		}

		public MathValue(int intValue) : this()
		{
			IntValue = intValue;
			ValueClass = ValueClassifier.Int;
		}

		public MathValue(float floatValue, bool isFractional) : this()
		{
			FloatValue = floatValue;
			ValueClass = isFractional ? ValueClassifier.FloatFractional : ValueClassifier.Float;
		}

		public MathValue(object objectValue) : this()
		{
			if (objectValue == null)
			{
				ValueClass = ValueClassifier.None;
			}
			else
			{
				var objectType = objectValue.GetType();

				if (objectType == typeof(int))
				{
					ValueClass = ValueClassifier.Int;
					IntValue = (int)objectValue;
				}
				else if (objectType == typeof(float))
				{
					ValueClass = ValueClassifier.Float;
					FloatValue = (float)objectValue;
				}
				else if (objectType == typeof(bool))
				{
					ValueClass = ValueClassifier.Boolean;
					BoolValue = (bool)objectValue;
				}
			}
		}

		public override string ToString()
		{
			switch (ValueClass)
			{
				case ValueClassifier.FloatFractional:
					return (FloatValue * 100).ToString() + "%";

				case ValueClassifier.Float:
					return FloatValue.ToString();

				case ValueClassifier.Boolean:
					return BoolValue.ToString();

				default:
					return "null";
			}
		}

		public static implicit operator MathValue(int value)
		{
			return new MathValue(value, false);
		}

		public static implicit operator MathValue(float value)
		{
			return new MathValue(value, false);
		}

		public static implicit operator MathValue(bool value)
		{
			return new MathValue(value);
		}
	}
}
