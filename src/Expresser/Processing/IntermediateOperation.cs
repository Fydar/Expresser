namespace Expresser.Processing
{
	public struct IntermediateOperation
	{
		public byte DistIndex;
		public IntermediateOperationCode OperationCode;
		public IntermediateParameter[] Parameters;

		public IntermediateOperation (byte distIndex, IntermediateOperationCode operationCode, IntermediateParameter[] parameters)
		{
			DistIndex = distIndex;
			OperationCode = operationCode;
			Parameters = parameters;
		}

		public override string ToString ()
		{
			return $"dist[{DistIndex}] = {OperationCode}: {string.Join (", ", Parameters)}";
		}
	}
}
