namespace Expresser.Processing
{
	public struct IntermediateParameter
	{
		public IntermediateSource Source;
		public byte Index;

		public IntermediateParameter (IntermediateSource source, byte index)
		{
			Source = source;
			Index = index;
		}

		public override string ToString ()
		{
			return $"{Source}[{Index}]";
		}
	}
}
