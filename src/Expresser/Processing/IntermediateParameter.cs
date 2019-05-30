namespace Expresser.Processing
{
	struct IntermediateParameter
	{
		public IntermediateSource Source;
		public byte Index;

		public IntermediateParameter (IntermediateSource source, byte index)
		{
			Source = source;
			Index = index;
		}
	}
}
