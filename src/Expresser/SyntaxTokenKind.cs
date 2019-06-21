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
		Not,
		Equal,
		NotEqual,
		GreaterThan,
		GreaterThanOrEqual,
		LessThan,
		LessThanOrEqual,

		// Suffix
		Percentage,

		// Data
		Value,
		Source,

		// Structure
		OpenParentheses,
		CloseParentheses,
		Comma,
	}
}
