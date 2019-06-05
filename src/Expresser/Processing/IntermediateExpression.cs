using System.Collections.Generic;

namespace Expresser.Processing
{

	public struct IntermediateExpression
	{
		public IntermediateOperation[] Operations;
		public MathValue[] Static;
		public int DistSize;


		struct CompilerBuffers
		{
			public readonly List<DistSpan> Dist;
			public readonly List<MathValue> Src;
			public readonly List<IntermediateOperation> Operations;
			public readonly List<IntermediateParameter> Parameters;

			public static CompilerBuffers New ()
			{
				return new CompilerBuffers (
					new List<DistSpan> (),
					new List<MathValue> (),
					new List<IntermediateOperation> (),
					new List<IntermediateParameter> ());
			}

			CompilerBuffers (List<DistSpan> dist, List<MathValue> src, List<IntermediateOperation> operations, List<IntermediateParameter> parameters)
			{
				Dist = dist;
				Src = src;
				Operations = operations;
				Parameters = parameters;
			}
		}

		public static IntermediateExpression Compile (ExpressionSyntax syntax)
		{
			var buffer = CompilerBuffers.New ();

			CompileSpan (buffer, syntax, 0, syntax.Tokens.Count);

			return new IntermediateExpression ()
			{
				Operations = buffer.Operations.ToArray (),
				Static = buffer.Src.ToArray(),
				DistSize = buffer.Dist.Count,
			};
		}

		static void CompileSpan(CompilerBuffers buffer, ExpressionSyntax syntax, int start, int length)
		{
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
							CompileSpan (buffer, syntax, parenthesesStart, i - parenthesesStart);
						}
						break;
				}
			}

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

					var lastIndex = DescribeIndex (syntax, buffer.Dist, buffer.Src, i - 1);
					var nextIndex = DescribeIndex (syntax, buffer.Dist, buffer.Src, i + 1);

					buffer.Parameters.Add (lastIndex);
					buffer.Parameters.Add (nextIndex);

					var currentSpan = Spread (buffer.Dist, (byte)(i - 1), 3);

					var intermediateOperationCode = (IntermediateOperationCode)token.Operation;

					var intermediateOperation = new IntermediateOperation (currentSpan.Index, intermediateOperationCode, buffer.Parameters.ToArray ());

					buffer.Parameters.Clear ();

					buffer.Operations.Add (intermediateOperation);
				}
			}
		}

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

		static IntermediateParameter DescribeIndex (ExpressionSyntax syntax, IReadOnlyList<DistSpan> distBuffer, List<MathValue> srcBuffer, int index)
		{
			for (byte i = 0; i < distBuffer.Count; i++)
			{
				var span = distBuffer[i];
				if (span.Contains (index))
				{
					return new IntermediateParameter (IntermediateSource.Output, i);
				}
			}

			var token = syntax.Tokens[index];

			int valueIndex = srcBuffer.LastIndexOf (token.Value);
			if (valueIndex == -1)
			{
				valueIndex = srcBuffer.Count;
				srcBuffer.Add (token.Value);
			}
			return new IntermediateParameter (IntermediateSource.Static, (byte)valueIndex);
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

		static DistSpan Spread(IList<DistSpan> distBuffer, byte start, byte length)
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

		struct DistSpan
		{
			public byte Length;
			public byte Start;
			public byte Index;

			public byte End
			{
				get
				{
					return (byte)(Start + Length);
				}
			}

			public static DistSpan None => new DistSpan ();

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
	}
}
