using Expresser.Language.SimpleMath.Runtime;
using Expresser.Lexing;
using System;
using System.Collections.Generic;

namespace Expresser.Language.SimpleMath.Compilation
{
	/// <summary>
	/// <para>A compiled form of a mathmatical expression.</para>
	/// </summary>
	public struct IntermediateExpression
	{
		private class CompilerBuffers
		{
			public readonly List<DistSpan> Dist;
			public readonly List<IntermediateOperation> Operations;
			public readonly List<IntermediateParameter> Parameters;
			public readonly List<MathValue> Src;

			public CompilerBuffers(
				List<DistSpan> dist,
				List<MathValue> src,
				List<IntermediateOperation> operations,
				List<IntermediateParameter> parameters)
			{
				Dist = dist;
				Src = src;
				Operations = operations;
				Parameters = parameters;
			}

			public static CompilerBuffers New()
			{
				return new CompilerBuffers(
					new List<DistSpan>(),
					new List<MathValue>(),
					new List<IntermediateOperation>(),
					new List<IntermediateParameter>());
			}
		}

		private struct DistSpan
		{
			public byte Index;
			public byte Length;
			public byte Start;
			public static DistSpan None => new DistSpan();

			public byte End
			{
				get
				{
					return (byte)(Start + Length);
				}
			}

			public DistSpan(byte start, byte length, byte index)
			{
				Start = start;
				Length = length;
				Index = index;
			}

			public bool Contains(int index)
			{
				return index >= Start && index <= Start + Length - 1;
			}

			public bool RangeEqual(DistSpan other) => Start == other.Start && Length == other.Length;
		}

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

		private enum OperatorPattern
		{
			None,
			Prefix,
			Conjective,
			Suffix,
		}

		private struct TokenReference : IComparable<TokenReference>
		{
			public int Index;
			public LexerToken Token;
			public TokenOperationCompiler Compiler;

			public TokenReference(int index, LexerToken token, TokenOperationCompiler compiler)
			{
				Index = index;
				Token = token;
				Compiler = compiler;
			}

			public int CompareTo(TokenReference other)
			{
				return Compiler.Order.CompareTo(other.Compiler.Order);
			}
		}

		private struct TokenOperationCompiler
		{
			public int Order;
			public OperatorPattern Pattern;

			public TokenOperationCompiler(int order, OperatorPattern pattern)
			{
				Order = order;
				Pattern = pattern;
			}
		}

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
			switch (parameter.Source)
			{
				case IntermediateSource.Static:
					return Static[parameter.Index];

				case IntermediateSource.Import:
					return Import[parameter.Index].Value;

				case IntermediateSource.ImportNegated:
					return Actions.Negate(Import[parameter.Index].Value);

				case IntermediateSource.Output:
					return dist[parameter.Index];
			}
			return new MathValue();
		}
	}
}
