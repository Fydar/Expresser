using System.Collections.Generic;

namespace Expresser.Processing
{
	struct IntermediateExpression
	{
		public IntermediateOperation[] Operations;

		public IntermediateExpression(IntermediateOperation[] operations)
		{
			Operations = operations;
		}

		public static IntermediateExpression Compile (ExpressionSyntax syntax)
		{
			var distBuffer = new List<DistSpan> ();
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

					var lastIndex = DescribeIndex (syntax, distBuffer, i - 1);
					var nextIndex = DescribeIndex (syntax, distBuffer, i + 1);

					var intermediateOperationCode = (IntermediateOperationCode)token.Operation;

					var intermediateOperation = new IntermediateOperation (intermediateOperationCode, paramBuffer.ToArray());

					paramBuffer.Clear ();

					opBuffer.Add (intermediateOperation);
				}
			}

			return new IntermediateExpression (opBuffer.ToArray ());
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

		static IntermediateParameter DescribeIndex (ExpressionSyntax syntax, IReadOnlyList<DistSpan> distBuffer, int index)
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
			return new IntermediateParameter (IntermediateSource.Static, (byte)index);
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

		struct DistSpan
		{
			public byte Length;
			public byte Start;
			public byte Index;

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
