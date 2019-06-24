namespace Expresser.Processing
{
	/// <summary>
	/// <para>A source for values within an <see cref="IntermediateExpression"/>.</para>
	/// </summary>
	public enum IntermediateSource
	{
		/// <summary>
		/// <para>The value is sourced from a collection of static values.</para>
		/// </summary>
		Static,

		/// <summary>
		/// <para>The value is sourced from an <see cref="IValueProvider"/>.</para>
		/// </summary>
		Import,

		/// <summary>
		/// <para>The value is sourced from an <see cref="IValueProvider"/> with a negated value.</para>
		/// </summary>
		ImportNegated,

		/// <summary>
		/// <para>The value is sourced from a buffer of outputted values.</para>
		/// </summary>
		Output,
	}
}
