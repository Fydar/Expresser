namespace Expresser
{
	public enum SyntaxTokenKind : byte
	{
		None,

		// Maths
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

		Value,
		Source,

		// Structure
		OpenParentheses,
		CloseParentheses,
		Comma,

		// Suffix
		Percentage,
	}
}
