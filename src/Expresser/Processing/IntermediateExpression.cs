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
		public IntermediateOperationActions Actions;

		private enum OperatorPattern
		{
			Prefix,
			Conjective,
			Suffix,
		}

		private struct TokenOperationCompiler
		{
			public SyntaxTokenKind TokenKind;
			public OperatorPattern Pattern;

			public TokenOperationCompiler (SyntaxTokenKind tokenKind, OperatorPattern pattern)
			{
				TokenKind = tokenKind;
				Pattern = pattern;
			}
		}

		private static readonly TokenOperationCompiler[] OrderOfOperations = new[]
		{
			new TokenOperationCompiler(SyntaxTokenKind.Percentage, OperatorPattern.Suffix),

			new TokenOperationCompiler(SyntaxTokenKind.Power, OperatorPattern.Conjective),
			new TokenOperationCompiler(SyntaxTokenKind.Divide, OperatorPattern.Conjective),
			new TokenOperationCompiler(SyntaxTokenKind.Multiply, OperatorPattern.Conjective),
			new TokenOperationCompiler(SyntaxTokenKind.Plus, OperatorPattern.Conjective),
			new TokenOperationCompiler(SyntaxTokenKind.Minus, OperatorPattern.Conjective),

			new TokenOperationCompiler(SyntaxTokenKind.GreaterThan, OperatorPattern.Conjective),
			new TokenOperationCompiler(SyntaxTokenKind.GreaterThanOrEqual, OperatorPattern.Conjective),
			new TokenOperationCompiler(SyntaxTokenKind.LessThan, OperatorPattern.Conjective),
			new TokenOperationCompiler(SyntaxTokenKind.LessThanOrEqual, OperatorPattern.Conjective),
			new TokenOperationCompiler(SyntaxTokenKind.Equal, OperatorPattern.Conjective),
			new TokenOperationCompiler(SyntaxTokenKind.NotEqual, OperatorPattern.Conjective),

			new TokenOperationCompiler(SyntaxTokenKind.And, OperatorPattern.Conjective),
			new TokenOperationCompiler(SyntaxTokenKind.Or, OperatorPattern.Conjective),
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
				Import = context.ResolveTerms (syntax.Terms),
				Actions = new IntermediateOperationActions(context)
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
						dist[operation.DistIndex] = Actions.Add (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Subtract:
						dist[operation.DistIndex] = Actions.Subtract (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Multiply:
						dist[operation.DistIndex] = Actions.Multiply (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Divide:
						dist[operation.DistIndex] = Actions.Divide (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Power:
						dist[operation.DistIndex] = Actions.Power (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.And:
						dist[operation.DistIndex] = Actions.And (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Or:
						dist[operation.DistIndex] = Actions.Or (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Equal:
						dist[operation.DistIndex] = Actions.Equal (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.NotEqual:
						dist[operation.DistIndex] = Actions.NotEqual (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.GreaterThan:
						dist[operation.DistIndex] = Actions.GreaterThan (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.GreaterThanOrEqual:
						dist[operation.DistIndex] = Actions.GreaterThanOrEqual (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.LessThan:
						dist[operation.DistIndex] = Actions.LessThan (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.LessThanOrEqual:
						dist[operation.DistIndex] = Actions.LessThanOrEqual (
							ParameterValue (operation.Parameters[0], dist),
							ParameterValue (operation.Parameters[1], dist)
						);
						break;

					case IntermediateOperationCode.Percentage:
						var suffixed = ParameterValue (operation.Parameters[0], dist);

						if (suffixed.ValueClass == ValueClassifier.Float)
						{
							dist[operation.DistIndex] = new MathValue (suffixed.FloatValue * 0.01f, true);
						}
						else
						{
							throw new InvalidOperationException ("Can't perform Percentage on target");
						}
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
				int interations = start + length;
				for (int i = start; i < interations; i++)
				{
					var token = syntax.Tokens[i];

					if (token.Operation != operation.TokenKind)
						continue;

					if (IsIndexCalculated (buffer.Dist, i))
						continue;

					DistSpan currentSpan;

					switch (operation.Pattern)
					{
						case OperatorPattern.Prefix:
						{
							var nextIndex = DescribeIndex (syntax, buffer, i + 1);

							buffer.Parameters.Add (nextIndex);

							currentSpan = Spread (buffer.Dist, (byte)i, 2);
							break;
						}
						case OperatorPattern.Conjective:
						{
							var lastIndex = DescribeIndex (syntax, buffer, i - 1);
							var nextIndex = DescribeIndex (syntax, buffer, i + 1);

							buffer.Parameters.Add (lastIndex);
							buffer.Parameters.Add (nextIndex);

							currentSpan = Spread (buffer.Dist, (byte)(i - 1), 3);

							break;
						}
						default:
						case OperatorPattern.Suffix:
						{
							var lastIndex = DescribeIndex (syntax, buffer, i - 1);

							buffer.Parameters.Add (lastIndex);

							currentSpan = Spread (buffer.Dist, (byte)(i - 1), 2);

							break;
						}
					}


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
					return Actions.Negate (Import[parameter.Index].Value);

				case IntermediateSource.Output:
					return dist[parameter.Index];
			}
			return new MathValue ();
		}
	}
}
