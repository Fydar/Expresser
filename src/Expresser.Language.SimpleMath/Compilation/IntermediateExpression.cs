using Expresser.Language.SimpleMath.Runtime;

namespace Expresser.Language.SimpleMath.Compilation
{
	/// <summary>
	/// <para>A compiled form of a mathmatical expression.</para>
	/// </summary>
	public struct IntermediateExpression
	{
		/// <summary>
		/// <para>The size the buffer required to invoke <c>Evaluate</c>.</para>
		/// </summary>
		public int DistSize;

		/// <summary>
		/// <para>An array of imported values for this expression.</para>
		/// </summary>
		public IValueProvider[] Import;

		/// <summary>
		/// <para>The body of this expression described by an array of operations.</para>
		/// </summary>
		public IntermediateOperation[] Operations;

		/// <summary>
		/// <para>A collection of static values used by the operations in this expression.</para>
		/// </summary>
		public MathValue[] Static;

		public IntermediateOperationActions Actions;

		/// <summary>
		/// <para>Evaluates this <see cref="IntermediateExpression"/> and return a singlular outputted value.</para>
		/// </summary>
		/// <param name="dist">A buffer used for calculations. Must be as large as this expressions <c>DistSize</c>.</param>
		/// <returns>
		/// <para>The output of the evaluation.</para>
		/// </returns>
		public MathValue Evaluate(MathValue[] dist)
		{
			for (int i = 0; i < Operations.Length; i++)
			{
				var operation = Operations[i];

				switch (operation.OperationCode)
				{
					case IntermediateOperationCode.Add:
						dist[operation.DistIndex] = Actions.Add(
							ParameterValue(operation.Parameters[0], dist),
							ParameterValue(operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Subtract:
						dist[operation.DistIndex] = Actions.Subtract(
							ParameterValue(operation.Parameters[0], dist),
							ParameterValue(operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Multiply:
						dist[operation.DistIndex] = Actions.Multiply(
							ParameterValue(operation.Parameters[0], dist),
							ParameterValue(operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Divide:
						dist[operation.DistIndex] = Actions.Divide(
							ParameterValue(operation.Parameters[0], dist),
							ParameterValue(operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Power:
						dist[operation.DistIndex] = Actions.Power(
							ParameterValue(operation.Parameters[0], dist),
							ParameterValue(operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.And:
						dist[operation.DistIndex] = Actions.And(
							ParameterValue(operation.Parameters[0], dist),
							ParameterValue(operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Or:
						dist[operation.DistIndex] = Actions.Or(
							ParameterValue(operation.Parameters[0], dist),
							ParameterValue(operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Not:
						dist[operation.DistIndex] = Actions.Not(
							ParameterValue(operation.Parameters[0], dist)
						);
						break;

					case IntermediateOperationCode.Equal:
						dist[operation.DistIndex] = Actions.Equal(
							ParameterValue(operation.Parameters[0], dist),
							ParameterValue(operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.NotEqual:
						dist[operation.DistIndex] = Actions.NotEqual(
							ParameterValue(operation.Parameters[0], dist),
							ParameterValue(operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.GreaterThan:
						dist[operation.DistIndex] = Actions.GreaterThan(
							ParameterValue(operation.Parameters[0], dist),
							ParameterValue(operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.GreaterThanOrEqual:
						dist[operation.DistIndex] = Actions.GreaterThanOrEqual(
							ParameterValue(operation.Parameters[0], dist),
							ParameterValue(operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.LessThan:
						dist[operation.DistIndex] = Actions.LessThan(
							ParameterValue(operation.Parameters[0], dist),
							ParameterValue(operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.LessThanOrEqual:
						dist[operation.DistIndex] = Actions.LessThanOrEqual(
							ParameterValue(operation.Parameters[0], dist),
							ParameterValue(operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Copy:
						dist[operation.DistIndex] = ParameterValue(operation.Parameters[0], dist);
						break;
				}
			}

			return dist[0];
		}

		private MathValue ParameterValue(IntermediateParameter parameter, MathValue[] dist)
		{
			return parameter.Source switch
			{
				IntermediateSource.Static => Static[parameter.Index],
				IntermediateSource.Import => Import[parameter.Index].Value,
				IntermediateSource.ImportNegated => Actions.Negate(Import[parameter.Index].Value),
				IntermediateSource.Output => dist[parameter.Index],
				_ => new MathValue(),
			};
		}
	}
}
