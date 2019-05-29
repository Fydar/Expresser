namespace Expresser.Processing
{
	enum IntermediateOperationCode
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
