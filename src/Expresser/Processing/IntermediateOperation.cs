namespace Expresser.Processing
{
	struct IntermediateOperation
	{
		public IntermediateOperationCode OperationCode;
		public IntermediateParameter[] Parameters;

		public IntermediateOperation (IntermediateOperationCode operationCode, IntermediateParameter[] parameters)
		{
			OperationCode = operationCode;
			Parameters = parameters;
		}
	}
}
