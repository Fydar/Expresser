namespace Expresser.Language.SimpleMath.Compilation
{
	/// <summary>
	/// <para>A code representing the type of operation that an <see cref="IntermediateOperation"/> represents.</para>
	/// </summary>
	public enum IntermediateOperationCode
	{
		None,

		Equal,
		NotEqual,
		LessThanOrEqual,
		GreaterThanOrEqual,
		GreaterThan,
		LessThan,

		Add,
		Subtract,
		Multiply,
		Divide,
		Power,

		And,
		Or,
		Not,

		Copy
	}
}
