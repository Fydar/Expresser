namespace Expresser
{
	public enum OperationCode : byte
	{
		None,

		Value,
		Source,

		// Structure
		OpenParentheses,
		CloseParentheses,

		// Maths
		Percentage,
		Plus,
		Minus,
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
	}
}
