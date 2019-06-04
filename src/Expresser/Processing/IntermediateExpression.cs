using System.Collections.Generic;

namespace Expresser.Processing
{
	public struct IntermediateExpression
	{
		public IntermediateOperation[] Operations;
		public MathValue[] Static;
		public int DistSize;

		public static IntermediateExpression Compile (ExpressionSyntax syntax)
		{
			var distBuffer = new List<DistSpan> ();
			var srcBuffer = new List<MathValue> ();

			var opBuffer = new List<IntermediateOperation> ();
			var paramBuffer = new List<IntermediateParameter> ();

			foreach (var operation in OrderOfOperations)
			{
				for (int i = 0 + 1; i < syntax.Tokens.Count; i++)
				{
					var token = syntax.Tokens[i];

					if (token.Operation != operation)
						continue;

					if (IsIndexCalculated (distBuffer, i))
						continue;

					var lastIndex = DescribeIndex (syntax, distBuffer, srcBuffer, i - 1);
					var nextIndex = DescribeIndex (syntax, distBuffer, srcBuffer, i + 1);

					paramBuffer.Add (lastIndex);
					paramBuffer.Add (nextIndex);

					var currentSpan = Spread (distBuffer, (byte)(i - 1), 3);

					var intermediateOperationCode = (IntermediateOperationCode)token.Operation;

					var intermediateOperation = new IntermediateOperation (currentSpan.Index, intermediateOperationCode, paramBuffer.ToArray ());

					paramBuffer.Clear ();

					opBuffer.Add (intermediateOperation);
				}
			}

			return new IntermediateExpression ()
			{
				Operations = opBuffer.ToArray (),
				Static = srcBuffer.ToArray(),
				DistSize = distBuffer.Count,
			};
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

				if (dist.End == start)
				{
					dist.Length += length;

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
