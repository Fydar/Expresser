namespace Expresser.Processing
{
	public enum IntermediateOperationCode
	{
		None,

		// Maths
		Add,
		Subtract,
		Multiply,
		Divide,
		Power,

		// Logic
		And,
		Or,
		Equals,
		NotEqual,
		GreaterThan,
		GreaterThanOrEqual,
		LessThan,
		LessThanOrEqual,

		Invoke,
	}
}
