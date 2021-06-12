using Expresser.Language.SimpleMath.Lexing;
using Expresser.Language.SimpleMath.Runtime;
using Expresser.Lexing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Expresser.Language.SimpleMath.Compilation
{
	/// <summary>
	/// <para>A compiled form of a mathmatical expression.</para>
	/// </summary>
	public static class IntermediateCompiler
	{
		private class CompilerBuffers
		{
			public readonly List<DistSpan> Dist;
			public readonly List<IntermediateOperation> Operations;
			public readonly List<IntermediateParameter> Parameters;
			public readonly List<MathValue> Src;
			public List<LexerToken>? ImportedTerms;

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
			public static DistSpan None => new();

			public byte index;
			public byte length;
			public byte start;

			public byte End => (byte)(start + length);

			public DistSpan(byte start, byte length, byte index)
			{
				this.start = start;
				this.length = length;
				this.index = index;
			}

			public bool Contains(int index)
			{
				return index >= start && index <= start + length - 1;
			}

			public bool RangeEqual(DistSpan other)
			{
				return start == other.start && length == other.length;
			}
		}

		private enum OperatorPattern
		{
			None,
			Prefix,
			Conjective,
			Suffix,
		}

		private struct TokenReference : IComparable<TokenReference>
		{
			public int index;
			public LexerToken token;
			public TokenOperationCompiler compiler;

			public TokenReference(int index, LexerToken token, TokenOperationCompiler compiler)
			{
				this.index = index;
				this.token = token;
				this.compiler = compiler;
			}

			public int CompareTo(TokenReference other)
			{
				return compiler.order.CompareTo(other.compiler.order);
			}
		}

		private struct TokenOperationCompiler
		{
			public int order;
			public OperatorPattern pattern;

			public TokenOperationCompiler(int order, OperatorPattern pattern)
			{
				this.order = order;
				this.pattern = pattern;
			}
		}

		private static readonly TokenOperationCompiler[] tokenCompilers = new[]
		{
			new TokenOperationCompiler(6, OperatorPattern.Conjective),
			new TokenOperationCompiler(6, OperatorPattern.Conjective),
			new TokenOperationCompiler(6, OperatorPattern.Conjective),
			new TokenOperationCompiler(6, OperatorPattern.Conjective),
			new TokenOperationCompiler(6, OperatorPattern.Conjective),
			new TokenOperationCompiler(6, OperatorPattern.Conjective),

			new TokenOperationCompiler(4, OperatorPattern.Conjective),
			new TokenOperationCompiler(5, OperatorPattern.Conjective),
			new TokenOperationCompiler(3, OperatorPattern.Conjective),
			new TokenOperationCompiler(2, OperatorPattern.Conjective),
			new TokenOperationCompiler(1, OperatorPattern.Conjective),
			new TokenOperationCompiler(7, OperatorPattern.Conjective),
			new TokenOperationCompiler(7, OperatorPattern.Conjective),

			new TokenOperationCompiler(7, OperatorPattern.Prefix),
		};

		/// <summary>
		/// <para>Compiles a new <see cref="IntermediateExpression"/> from a source expression.</para>
		/// </summary>
		/// <param name="syntax">The parsed string that describes an expression to compile.</param>
		/// <param name="context">The compilation context.</param>
		/// <returns></returns>
		public static IntermediateExpression Compile(string expression, IMathContext? context = null)
		{
			var buffer = CompilerBuffers.New();

			var lexer = SimpleMathLexerLanguage.Lexer;
			var syntax = lexer.Tokenize(expression)
				.Where(token => !IsWhitespace(token))
				.ToArray();

			buffer.ImportedTerms = syntax.Where(token => IsIdentifier(token)).ToList();

			CompileSpan(expression, buffer, syntax, 0, syntax.Length);

			return new IntermediateExpression()
			{
				Operations = buffer.Operations.ToArray(),
				Static = buffer.Src.ToArray(),
				DistSize = buffer.Dist.Count,
				Import = context.ResolveTerms(buffer.ImportedTerms
					.Select(token => expression.Substring(token.StartIndex, token.Length))
					.ToList()) ?? Array.Empty<IValueProvider>(),
				Actions = new IntermediateOperationActions(context)
			};
		}

		private static int CompileSpan(string expression, CompilerBuffers buffer, LexerToken[] syntax, int start, int length)
		{
			if (length == 0)
			{
				throw new InvalidOperationException("Trying to calculate with 0 length span");
			}
			if (length == 1)
			{
				var singleParameter = DescribeIndex(expression, syntax, buffer, start);
				var singleSpan = Spread(buffer.Dist, (byte)start, 1);

				buffer.Parameters.Add(singleParameter);

				var intermediateOperationCode = IntermediateOperationCode.Copy;
				var intermediateOperation = new IntermediateOperation(singleSpan.index, intermediateOperationCode, buffer.Parameters.ToArray());

				buffer.Parameters.Clear();
				buffer.Operations.Add(intermediateOperation);
				return singleSpan.index;
			}

			int spanEnd = start + length;
			int depth = 0;
			int parenthesesStart = -1;
			for (int i = start; i < spanEnd; i++)
			{
				var token = syntax[i];
				if (IsOpeningParentheses(token))
				{
					if (++depth == 1)
					{
						parenthesesStart = i + 1;
					}
				}
				else if (IsClosingParentheses(token))
				{
					if (--depth == 0)
					{
						int growIndex = CompileSpan(expression, buffer, syntax, parenthesesStart, i - parenthesesStart);

						Grow(buffer.Dist, growIndex);
					}
				}
			}

			int interations = start + length;
			var operatorTokens = new List<TokenReference>(interations);
			for (int i = start; i < interations; i++)
			{
				var token = syntax[i];

				if (IsOperator(token))
				{
					var compiler = tokenCompilers[token.Classifier - 7];
					if (compiler.pattern != OperatorPattern.None)
					{
						operatorTokens.Add(new TokenReference(i, token, compiler));
					}
				}
			}
			operatorTokens.Sort();

			int distIndex = -1;
			for (int k = 0; k < operatorTokens.Count; k++)
			{
				var tokenReference = operatorTokens[k];
				int i = tokenReference.index;
				var token = tokenReference.token;

				if (IsIndexCalculated(buffer.Dist, i))
				{
					continue;
				}

				DistSpan currentSpan;
				switch (tokenReference.compiler.pattern)
				{
					case OperatorPattern.Prefix:
					{
						var nextIndex = DescribeIndex(expression, syntax, buffer, i + 1);

						buffer.Parameters.Add(nextIndex);

						currentSpan = Spread(buffer.Dist, (byte)i, 2);
						break;
					}
					case OperatorPattern.Conjective:
					{
						var lastIndex = DescribeIndex(expression, syntax, buffer, i - 1);
						var nextIndex = DescribeIndex(expression, syntax, buffer, i + 1);

						buffer.Parameters.Add(lastIndex);
						buffer.Parameters.Add(nextIndex);

						currentSpan = Spread(buffer.Dist, (byte)(i - 1), 3);

						break;
					}
					default:
					case OperatorPattern.Suffix:
					{
						var lastIndex = DescribeIndex(expression, syntax, buffer, i - 1);

						buffer.Parameters.Add(lastIndex);

						currentSpan = Spread(buffer.Dist, (byte)(i - 1), 2);

						break;
					}
				}

				distIndex = currentSpan.index;

				var intermediateOperationCode = (IntermediateOperationCode)token.Classifier - 6;

				var intermediateOperation = new IntermediateOperation(currentSpan.index, intermediateOperationCode, buffer.Parameters.ToArray());

				buffer.Parameters.Clear();

				buffer.Operations.Add(intermediateOperation);
			}

			return distIndex;
		}

		private static IntermediateParameter DescribeIndex(string expression, LexerToken[] syntax, CompilerBuffers buffers, int index)
		{
			for (byte i = 0; i < buffers.Dist.Count; i++)
			{
				var span = buffers.Dist[i];
				if (span.Contains(index))
				{
					return new IntermediateParameter(IntermediateSource.Output, i);
				}
			}

			var token = syntax[index];

			if (IsIdentifier(token))
			{
				int importIndex = buffers.ImportedTerms?.IndexOf(token) ?? -1;
				if (importIndex == -1)
				{
					throw new InvalidOperationException($"Could not resolve the term \"{expression.Substring(token.StartIndex, token.Length)}\"");
				}

				return new IntermediateParameter(IntermediateSource.Import, (byte)importIndex);
			}
			else
			{
				MathValue value;
				if (IsNumericLiteral(token))
				{
					value = new MathValue(float.Parse(expression.Substring(token.StartIndex, token.Length)));
				}
				else if (IsFalseLiteral(token))
				{
					value = new MathValue(false);
				}
				else if (IsTrueLiteral(token))
				{
					value = new MathValue(true);
				}
				else
				{
					throw new InvalidOperationException(string.Format("Unexpected token \"{0}\"", expression.Substring(token.StartIndex, token.Length)));
				}

				int valueIndex = buffers.Src.LastIndexOf(value);
				if (valueIndex == -1)
				{
					valueIndex = buffers.Src.Count;
					buffers.Src.Add(value);
				}
				return new IntermediateParameter(IntermediateSource.Static, (byte)valueIndex);
			}
		}

		private static DistSpan Grow(IList<DistSpan> distBuffer, int distIndex)
		{
			var dist = distBuffer[distIndex];

			dist.start -= 1;
			dist.length += 2;

			distBuffer[distIndex] = dist;
			return dist;
		}

		private static bool IsIndexCalculated(IReadOnlyList<DistSpan> distBuffer, int index)
		{
			foreach (var span in distBuffer)
			{
				if (span.Contains(index))
				{
					return true;
				}
			}
			return false;
		}

		private static DistSpan Spread(IList<DistSpan> distBuffer, byte start, byte length)
		{
			byte end = (byte)(start + length);
			for (int i = 0; i < distBuffer.Count; i++)
			{
				var dist = distBuffer[i];
				if (dist.start == end)
				{
					dist.start -= length;
					dist.length += length;

					distBuffer[i] = dist;
					return dist;
				}
				if (dist.start == end - 1)
				{
					dist.start -= (byte)(length - 1);
					dist.length += (byte)(length - 1);

					distBuffer[i] = dist;
					return dist;
				}

				if (dist.End == start)
				{
					dist.length += length;

					distBuffer[i] = dist;
					return dist;
				}
				if (dist.End == start + 1)
				{
					dist.length += (byte)(length - 1);

					distBuffer[i] = dist;
					return dist;
				}
			}

			var newDist = new DistSpan(start, length, (byte)distBuffer.Count);
			distBuffer.Add(newDist);
			return newDist;
		}

		private static bool IsTrueLiteral(LexerToken token) => token.Classifier == 0;
		private static bool IsFalseLiteral(LexerToken token) => token.Classifier == 1;
		private static bool IsNumericLiteral(LexerToken token) => token.Classifier == 2;
		private static bool IsIdentifier(LexerToken token) => token.Classifier == 3;
		private static bool IsWhitespace(LexerToken token) => token.Classifier == 4;
		private static bool IsOpeningParentheses(LexerToken token) => token.Classifier == 5;
		private static bool IsClosingParentheses(LexerToken token) => token.Classifier == 6;
		private static bool IsOperator(LexerToken token) => token.Classifier > 6;
	}
}
