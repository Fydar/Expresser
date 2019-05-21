using System;
using System.Collections.Generic;
using System.Text;

namespace Expresser
{
	/// <summary>
	/// <para>A mathematical expression in a compiled format.</para>
	/// </summary>
	public class CompiledExpression
	{
		enum SpanClassifier : byte
		{
			None,
			Numeric,
			Operator,
			String,
			Space,
			Structure
		}

		struct CalculatedSpan
		{
			public int Length;
			public int Start;
			public MathValue Value;
			public static CalculatedSpan None => new CalculatedSpan ();

			public CalculatedSpan (int start, int length, MathValue value)
			{
				Start = start;
				Length = length;
				Value = value;
			}

			public bool Contains (int index)
			{
				return index >= Start && index <= Start + Length - 1;
			}

			public bool RangeEqual (CalculatedSpan other) => Start == other.Start && Length == other.Length;
		}

		private struct CharacterDescriptor
		{
			public char Character;
			public short Index;
			private readonly string SourceString;

			public bool IsOperator => Character == '+' ||
						Character == '-' ||
						Character == '*' ||
						Character == '/' ||
						Character == '^' ||
						Character == '&' ||
						Character == '%' ||
						Character == '|' ||
						Character == '=' ||
						Character == '!' ||
						Character == '>' ||
						Character == '<';

			public bool IsStructure => Character == '(' ||
						Character == ')';

			public OperationCode OperationCode
			{
				get
				{
					switch (Character)
					{
						case '+': return OperationCode.Plus;
						case '-': return OperationCode.Minus;
						case '*': return OperationCode.Multiply;
						case '/': return OperationCode.Divide;
						case '^': return OperationCode.Power;
						case '%': return OperationCode.Percentage;
						case '(': return OperationCode.OpenParentheses;
						case ')': return OperationCode.CloseParentheses;
						case '>': return OperationCode.GreaterThan;
						case '<': return OperationCode.LessThan;
						default: return OperationCode.None;
					}
				}
			}

			public SpanClassifier Type
			{
				get
				{
					if (Index < 0 || char.IsWhiteSpace (Character))
					{
						return SpanClassifier.Space;
					}
					else if (char.IsDigit (Character) || Character == '.')
					{
						return SpanClassifier.Numeric;
					}
					else if (char.IsLetter (Character))
					{
						return SpanClassifier.String;
					}
					else if (IsStructure)
					{
						return SpanClassifier.Structure;
					}
					else if (IsOperator)
					{
						return SpanClassifier.Operator;
					}
					else
					{
						throw new InvalidOperationException (string.Format ("Character \"{0}\" could not be classified", Character));
					}
				}
			}

			public CharacterDescriptor (string sourceString, short index)
			{
				SourceString = sourceString;
				Index = index;
				if (Index >= 0)
				{
					if (index >= sourceString.Length)
					{
						Index = -2;
						Character = new char ();
					}
					else
					{
						Character = sourceString[index];
					}
				}
				else
				{
					Index = -1;
					Character = new char ();
				}
			}

			public static CharacterDescriptor End (string sourceString)
			{
				return new CharacterDescriptor (sourceString, -2);
			}

			public static CharacterDescriptor Start (string sourceString)
			{
				return new CharacterDescriptor (sourceString, -1);
			}

			public CharacterDescriptor Next ()
			{
				if (Index == -2)
					return this;
				return new CharacterDescriptor (SourceString, (short)(Index + 1));
			}

			public override string ToString ()
			{
				if (Index == -1)
					return "Start";
				if (Index == -2)
					return "End";

				return $"\"{Character}\" at {Index}";
			}
		}
		struct IndexDescriptor
		{
			public ExpressionToken Token;
			public MathValue Value;

			public IndexDescriptor (MathValue value, ExpressionToken token)
			{
				Value = value;
				Token = token;
			}
		}
		private static readonly OperationCode[] OrderOfOperations = new[]
						{
			OperationCode.Percentage,

			OperationCode.Power,
			OperationCode.Divide,
			OperationCode.Multiply,
			OperationCode.Plus,
			OperationCode.Minus,

			OperationCode.GreaterThan,
			OperationCode.GreaterThanOrEqual,
			OperationCode.LessThan,
			OperationCode.LessThanOrEqual,
			OperationCode.Equal,
			OperationCode.NotEqual,

			OperationCode.And,
			OperationCode.Or,
		};

		private readonly List<CalculatedSpan> Buffer;
		private readonly IValueProvider[] Sources;

		/// <summary>
		/// <para></para>
		/// </summary>
		public IMathContext Context { get; private set; }

		/// <summary>
		/// <para></para>
		/// </summary>
		public ExpressionSyntax Syntax { get; private set; }

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="context"></param>
		public CompiledExpression (string expression, IMathContext context = null)
			: this (new ExpressionSyntax (expression), context)
		{
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="syntax"></param>
		/// <param name="context"></param>
		public CompiledExpression (ExpressionSyntax syntax, IMathContext context = null)
		{
			Syntax = syntax;
			Context = context;

			if (syntax.Terms != null && syntax.Terms.Count > 0)
			{
				if (context == null)
				{
					throw new InvalidOperationException ("Cannot resolve terms used in expression without IMathContext");
				}
				Sources = context.ResolveTerms (syntax.Terms);
			}

			Buffer = new List<CalculatedSpan> ();
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		public MathValue Evaluate ()
		{
			return Evaluate (Syntax.Tokens);
		}

		public override string ToString ()
		{
			var sb = new StringBuilder ();

			var lastToken = Syntax.Tokens[0];
			for (int i = 1; i < Syntax.Tokens.Count; i++)
			{
				var token = Syntax.Tokens[i];
				sb.Append (lastToken);
				if (lastToken.Operation != OperationCode.OpenParentheses
					&& token.Operation != OperationCode.CloseParentheses
					&& token.Operation != OperationCode.Percentage)
				{
					sb.Append (' ');
				}
				lastToken = token;
			}
			sb.Append (Syntax.Tokens[Syntax.Tokens.Count - 1]);
			return sb.ToString ();
		}

		IndexDescriptor DescribeIndex (IReadOnlyList<ExpressionToken> tokens, int index)
		{
			for (int j = 0; j < Buffer.Count; j++)
			{
				var span = Buffer[j];
				if (span.Contains (index))
				{
					return new IndexDescriptor (span.Value, ExpressionToken.None);
				}
			}

			var token = tokens[index];
			return new IndexDescriptor (token.ValueWithSources (Sources), token);
		}

		IndexDescriptor DescribeIndex (IReadOnlyList<ExpressionToken> tokens, int index, out CalculatedSpan span, out int spanIndex)
		{
			for (int j = 0; j < Buffer.Count; j++)
			{
				span = Buffer[j];
				if (span.Contains (index))
				{
					spanIndex = j;
					return new IndexDescriptor (span.Value, ExpressionToken.None);
				}
			}
			spanIndex = -1;
			span = CalculatedSpan.None;

			var token = tokens[index];
			return new IndexDescriptor (token.ValueWithSources (Sources), token);
		}

		MathValue Evaluate (IReadOnlyList<ExpressionToken> tokens)
		{
			Buffer.Clear ();
			return Evaluate (tokens, 0, tokens.Count);
		}

		MathValue Evaluate (IReadOnlyList<ExpressionToken> tokens, int spanStart, int spanLength)
		{
			if (spanLength == 0)
			{
				throw new InvalidOperationException ("Trying to calculate with 0 length span");
			}
			if (spanLength == 1)
			{
				var onlyIndex = DescribeIndex (tokens, spanStart);

				switch (onlyIndex.Token.Operation)
				{
					case OperationCode.Source:
					case OperationCode.Value:
						Buffer.Add (new CalculatedSpan (spanStart, 1, onlyIndex.Value));
						break;

					default:
						throw new InvalidOperationException ("Operator found with no values.");
				}
			}
			else
			{
				int spanEnd = spanStart + spanLength;
				int depth = 0;
				int parenthesesStart = -1;
				for (int i = spanStart; i < spanEnd; i++)
				{
					switch (tokens[i].Operation)
					{
						case OperationCode.OpenParentheses:
							if (++depth == 1)
							{
								parenthesesStart = i + 1;
							}
							break;

						case OperationCode.CloseParentheses:
							if (--depth == 0)
							{
								Evaluate (tokens, parenthesesStart, i - parenthesesStart);
							}
							break;
					}
				}

				foreach (var operation in OrderOfOperations)
				{
					int interations = spanStart + spanLength - 1;
					for (int i = spanStart + 1; i < interations; i++)
					{
						int thisTokenSpanIndex;
						CalculatedSpan thisTokenSpan;

						var thisIndex = DescribeIndex (tokens, i, out thisTokenSpan, out thisTokenSpanIndex);

						if (operation != thisIndex.Token.Operation)
							continue;

						int lastTokenSpanIndex;
						CalculatedSpan lastTokenSpan;

						int nextTokenSpanIndex;
						CalculatedSpan nextTokenSpan;

						var lastIndex = DescribeIndex (tokens, i - 1, out lastTokenSpan, out lastTokenSpanIndex);
						var nextIndex = DescribeIndex (tokens, i + 1, out nextTokenSpan, out nextTokenSpanIndex);

						MathValue value;

						switch (operation)
						{
							case OperationCode.Plus:
								value = MathValue.Add (lastIndex.Value, nextIndex.Value);
								break;

							case OperationCode.Multiply:
								value = MathValue.Multiply (lastIndex.Value, nextIndex.Value);
								break;

							case OperationCode.Minus:
								value = MathValue.Subtract (lastIndex.Value, nextIndex.Value);
								break;

							case OperationCode.Divide:
								value = MathValue.Divide (lastIndex.Value, nextIndex.Value);
								break;

							case OperationCode.Power:
								value = MathValue.Power (lastIndex.Value, nextIndex.Value);
								break;


							case OperationCode.GreaterThan:
								value = MathValue.GreaterThan (lastIndex.Value, nextIndex.Value);
								break;

							case OperationCode.GreaterThanOrEqual:
								value = MathValue.GreaterThanOrEqual (lastIndex.Value, nextIndex.Value);
								break;

							case OperationCode.LessThan:
								value = MathValue.LessThan (lastIndex.Value, nextIndex.Value);
								break;

							case OperationCode.LessThanOrEqual:
								value = MathValue.LessThanOrEqual (lastIndex.Value, nextIndex.Value);
								break;

							case OperationCode.Equal:
								value = MathValue.Equal (lastIndex.Value, nextIndex.Value);
								break;

							case OperationCode.NotEqual:
								value = MathValue.NotEqual (lastIndex.Value, nextIndex.Value);
								break;

							case OperationCode.And:
								value = MathValue.And (lastIndex.Value, nextIndex.Value);
								break;

							case OperationCode.Or:
								value = MathValue.Or (lastIndex.Value, nextIndex.Value);
								break;

							default:
								continue;
						}

						if (!lastTokenSpan.RangeEqual (CalculatedSpan.None))
						{
							if (!nextTokenSpan.RangeEqual (CalculatedSpan.None))
							{
								Buffer[lastTokenSpanIndex] = new CalculatedSpan (
									lastTokenSpan.Start,
									lastTokenSpan.Length + nextTokenSpan.Length + 1,
									value);

								Buffer.RemoveAt (nextTokenSpanIndex);
							}
							else
							{
								Buffer[lastTokenSpanIndex] = new CalculatedSpan (
									lastTokenSpan.Start,
									lastTokenSpan.Length + 2,
									value);
							}
						}
						else
						{
							if (!nextTokenSpan.RangeEqual (CalculatedSpan.None))
							{
								Buffer[nextTokenSpanIndex] = new CalculatedSpan (nextTokenSpan.Start - 2, nextTokenSpan.Length + 2, value);
							}
							else
							{
								Buffer.Add (new CalculatedSpan (i - 1, 3, value));
							}
						}
					}
				}
			}

			if (Buffer.Count == 0)
			{
				throw new InvalidOperationException ("No operators found");
			}
			else if (Buffer.Count != 1)
			{
				throw new InvalidOperationException ("Missing operators resulting in dangling expression");
			}

			CalculatedSpan finalSpan;
			int finalSpanIndex;
			DescribeIndex (tokens, spanStart, out finalSpan, out finalSpanIndex);

			// Have we covered the area we have been told to calculate?
			if (finalSpan.Start != spanStart && finalSpan.Length != spanLength)
			{
				throw new InvalidOperationException ("Dangling tokens uncaught by operators");
			}

			// Expand span if it encompasses parenthasis.
			if (finalSpan.Start != 0
				&& finalSpan.Start + finalSpan.Length < tokens.Count
				&& finalSpan.Start + finalSpan.Length < tokens.Count)
			{
				var beforeSpan = tokens[finalSpan.Start - 1].Operation;
				if (beforeSpan == OperationCode.OpenParentheses)
				{
					finalSpan = new CalculatedSpan (
						finalSpan.Start - 1,
						finalSpan.Length + 2,
						finalSpan.Value);
					Buffer[finalSpanIndex] = finalSpan;
				}
			}

			return finalSpan.Value;
		}
	}
}
