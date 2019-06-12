using System;
using System.Collections.Generic;

namespace Expresser.Processing
{
	public struct IntermediateExpression
	{
		struct CompilerBuffers
		{
			public readonly List<DistSpan> Dist;
			public readonly List<IntermediateOperation> Operations;
			public readonly List<IntermediateParameter> Parameters;
			public readonly List<MathValue> Src;

			public CompilerBuffers (
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

			public static CompilerBuffers New ()
			{
				return new CompilerBuffers (
					new List<DistSpan> (),
					new List<MathValue> (),
					new List<IntermediateOperation> (),
					new List<IntermediateParameter> ());
			}
		}

		struct DistSpan
		{
			public byte Index;
			public byte Length;
			public byte Start;
			public static DistSpan None => new DistSpan ();

			public byte End
			{
				get
				{
					return (byte)(Start + Length);
				}
			}

			public DistSpan (byte start, byte length, byte index)
			{
				Start = start;
				Length = length;
				Index = index;
			}

			public bool Contains (int index)
			{
				return index >= Start && index <= Start + Length - 1;
			}

			public bool RangeEqual (DistSpan other) => Start == other.Start && Length == other.Length;
		}

		public int DistSize;
		public IValueProvider[] Import;
		public IntermediateOperation[] Operations;
		public MathValue[] Static;

		private static readonly SyntaxTokenKind[] OrderOfOperations = new[]
		{
			SyntaxTokenKind.Percentage,

			SyntaxTokenKind.Power,
			SyntaxTokenKind.Divide,
			SyntaxTokenKind.Multiply,
			SyntaxTokenKind.Plus,
			SyntaxTokenKind.Minus,

			SyntaxTokenKind.GreaterThan,
			SyntaxTokenKind.GreaterThanOrEqual,
			SyntaxTokenKind.LessThan,
			SyntaxTokenKind.LessThanOrEqual,
			SyntaxTokenKind.Equal,
			SyntaxTokenKind.NotEqual,

			SyntaxTokenKind.And,
			SyntaxTokenKind.Or,
		};

		public static IntermediateExpression Compile (ExpressionSyntax syntax, IMathContext context = null)
		{
			var buffer = CompilerBuffers.New ();

			CompileSpan (buffer, syntax, 0, syntax.Tokens.Count);

			return new IntermediateExpression ()
			{
				Operations = buffer.Operations.ToArray (),
				Static = buffer.Src.ToArray (),
				DistSize = buffer.Dist.Count,
				Import = context.ResolveTerms (syntax.Terms)
			};
		}

		public MathValue Evaluate (MathValue[] dist)
		{

			for (int i = 0; i < Operations.Length; i++)
			{
				var operation = Operations[i];

				switch (operation.OperationCode)
				{
					case IntermediateOperationCode.Add:
						dist[operation.DistIndex] = MathValue.Add (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Subtract:
						dist[operation.DistIndex] = MathValue.Subtract (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Multiply:
						dist[operation.DistIndex] = MathValue.Multiply (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Divide:
						dist[operation.DistIndex] = MathValue.Divide (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Power:
						dist[operation.DistIndex] = MathValue.Power (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.And:
						dist[operation.DistIndex] = MathValue.And (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Or:
						dist[operation.DistIndex] = MathValue.Or (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Equal:
						dist[operation.DistIndex] = MathValue.Equal (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.NotEqual:
						dist[operation.DistIndex] = MathValue.NotEqual (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.GreaterThan:
						dist[operation.DistIndex] = MathValue.GreaterThan (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.GreaterThanOrEqual:
						dist[operation.DistIndex] = MathValue.GreaterThanOrEqual (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.LessThan:
						dist[operation.DistIndex] = MathValue.LessThan (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.LessThanOrEqual:
						dist[operation.DistIndex] = MathValue.LessThanOrEqual (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Invoke:
						break;

					case IntermediateOperationCode.Copy:
						dist[operation.DistIndex] = ParameterValue (operation.Parameters[0], dist);
						break;
				}
			}

			return dist[0];
		}

		static int CompileSpan (CompilerBuffers buffer, ExpressionSyntax syntax, int start, int length)
		{
			if (length == 0)
			{
				throw new InvalidOperationException ("Trying to calculate with 0 length span");
			}
			if (length == 1)
			{
				var singleParameter = DescribeIndex (syntax, buffer, start);
				var singleSpan = Spread (buffer.Dist, (byte)start, 1);

				buffer.Parameters.Add (singleParameter);

				var intermediateOperationCode = IntermediateOperationCode.Copy;
				var intermediateOperation = new IntermediateOperation (singleSpan.Index, intermediateOperationCode, buffer.Parameters.ToArray ());

				buffer.Parameters.Clear ();
				buffer.Operations.Add (intermediateOperation);
				return singleSpan.Index;
			}

			int spanEnd = start + length;
			int depth = 0;
			int parenthesesStart = -1;
			for (int i = start; i < spanEnd; i++)
			{
				switch (syntax.Tokens[i].Operation)
				{
					case SyntaxTokenKind.OpenParentheses:
						if (++depth == 1)
						{
							parenthesesStart = i + 1;
						}
						break;

					case SyntaxTokenKind.CloseParentheses:
						if (--depth == 0)
						{
							int growIndex = CompileSpan (buffer, syntax, parenthesesStart, i - parenthesesStart);

							Grow (buffer.Dist, growIndex);
						}
						break;
				}
			}

			int distIndex = -1;
			foreach (var operation in OrderOfOperations)
			{
				int interations = start + length - 1;
				for (int i = start + 1; i < interations; i++)
				{
					var token = syntax.Tokens[i];

					if (token.Operation != operation)
						continue;

					if (IsIndexCalculated (buffer.Dist, i))
						continue;

					var lastIndex = DescribeIndex (syntax, buffer, i - 1);
					var nextIndex = DescribeIndex (syntax, buffer, i + 1);

					buffer.Parameters.Add (lastIndex);
					buffer.Parameters.Add (nextIndex);

					var currentSpan = Spread (buffer.Dist, (byte)(i - 1), 3);

					distIndex = currentSpan.Index;

					var intermediateOperationCode = (IntermediateOperationCode)token.Operation;

					var intermediateOperation = new IntermediateOperation (currentSpan.Index, intermediateOperationCode, buffer.Parameters.ToArray ());

					buffer.Parameters.Clear ();

					buffer.Operations.Add (intermediateOperation);
				}
			}
			return distIndex;
		}

		static IntermediateParameter DescribeIndex (ExpressionSyntax syntax, CompilerBuffers buffers, int index)
		{
			for (byte i = 0; i < buffers.Dist.Count; i++)
			{
				var span = buffers.Dist[i];
				if (span.Contains (index))
				{
					return new IntermediateParameter (IntermediateSource.Output, i);
				}
			}

			var token = syntax.Tokens[index];

			if (token.Operation == SyntaxTokenKind.Value)
			{
				int valueIndex = buffers.Src.LastIndexOf (token.Value);
				if (valueIndex == -1)
				{
					valueIndex = buffers.Src.Count;
					buffers.Src.Add (token.Value);
				}
				return new IntermediateParameter (IntermediateSource.Static, (byte)valueIndex);
			}

			if (token.Operation == SyntaxTokenKind.Source)
			{
				if (token.Multiplier == -1)
				{
					return new IntermediateParameter (IntermediateSource.ImportNegated, token.Source);
				}
				else
				{
					return new IntermediateParameter (IntermediateSource.Import, token.Source);
				}
			}

			throw new InvalidOperationException (string.Format ("Unrecognised token {0}", token));
		}

		static DistSpan Grow (IList<DistSpan> distBuffer, int distIndex)
		{
			var dist = distBuffer[distIndex];

			dist.Start -= 1;
			dist.Length += 2;

			distBuffer[distIndex] = dist;
			return dist;
		}

		static bool IsIndexCalculated (IReadOnlyList<DistSpan> distBuffer, int index)
		{
			foreach (var span in distBuffer)
			{
				if (span.Contains (index))
					return true;
			}
			return false;
		}

		static DistSpan Spread (IList<DistSpan> distBuffer, byte start, byte length)
		{
			byte end = (byte)(start + length);
			for (int i = 0; i < distBuffer.Count; i++)
			{
				var dist = distBuffer[i];
				if (dist.Start == end)
				{
					dist.Start -= length;
					dist.Length += length;

					distBuffer[i] = dist;
					return dist;
				}
				if (dist.Start == end - 1)
				{
					dist.Start -= (byte)(length - 1);
					dist.Length += (byte)(length - 1);

					distBuffer[i] = dist;
					return dist;
				}

				if (dist.End == start)
				{
					dist.Length += length;

					distBuffer[i] = dist;
					return dist;
				}
				if (dist.End == start + 1)
				{
					dist.Length += (byte)(length - 1);

					distBuffer[i] = dist;
					return dist;
				}
			}

			var newDist = new DistSpan (start, length, (byte)distBuffer.Count);
			distBuffer.Add (newDist);
			return newDist;
		}

		MathValue ParameterValue (IntermediateParameter parameter, MathValue[] dist)
		{
			switch (parameter.Source)
			{
				case IntermediateSource.Static:
					return Static[parameter.Index];

				case IntermediateSource.Import:
					return Import[parameter.Index].Value;

				case IntermediateSource.ImportNegated:
					return MathValue.Negate(Import[parameter.Index].Value);

				case IntermediateSource.Output:
					return dist[parameter.Index];
			}
			return new MathValue ();
		}
	}
}
