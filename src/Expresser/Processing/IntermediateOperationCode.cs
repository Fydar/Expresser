namespace Expresser.Processing
{
	/// <summary>
	/// <para>A code representing the type of operation that an <see cref="IntermediateOperation"/> represents.</para>
	/// </summary>
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
		Not,
		Equal,
		NotEqual,
		GreaterThan,
		GreaterThanOrEqual,
		LessThan,
		LessThanOrEqual,

		Percentage,

		Invoke,
		Copy
	}
}
