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
		Equal,
		NotEqual,
		GreaterThan,
		GreaterThanOrEqual,
		LessThan,
		LessThanOrEqual,

		Invoke,
		Copy
	}
}
