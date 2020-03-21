namespace Expresser.Processing
{
	/// <summary>
	/// <para>A parameter in an <see cref="IntermediateExpression"/>.</para>
	/// </summary>
	public struct IntermediateParameter
	{
		/// <summary>
		/// <para>The soure from where this parameters value is sourced from.</para>
		/// </summary>
		public readonly IntermediateSource Source;

		/// <summary>
		/// <para>The location within the source where this parameters value is sourced from.</para>
		/// </summary>
		public readonly byte Index;

		/// <summary>
		/// <para>Constructs a new instance of the <see cref="IntermediateParameter"/>.</para>
		/// </summary>
		/// <param name="source">The soure from where this parameters value is sourced from.</param>
		/// <param name="index">The index within the source to source a value from.</param>
		public IntermediateParameter(IntermediateSource source, byte index)
		{
			Source = source;
			Index = index;
		}

		/// <summary>
		/// <para>Converts this <see cref="IntermediateParameter"/> into a string representation.</para>
		/// </summary>
		/// <returns>
		/// <para>A string representation of this <see cref="IntermediateParameter"/>.</para>
		/// </returns>
		public override string ToString()
		{
			return $"{Source}[{Index}]";
		}
	}
}
