using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Expresser
{
	/// <summary>
	/// <para>A mathematical expression in a compiled format.</para>
	/// </summary>
	public class CompiledExpression
	{
		/// <summary>
		/// <para></para>
		/// </summary>
		public enum OperationCode : byte
		{
			None,

			Value,
			Source,

			// Structure
			OpenParentheses,
			CloseParentheses,

			// Maths
			Percentage,
			Plus,
			Minus,
			Multiply,
			Divide,
			Power,

			// Logic
			And,
			Or,
			Equal,
			NotEqual,
			GreaterThan,
			GreaterThanOrEqual,
			LessThan,
			LessThanOrEqual,
		}

		enum SpanClassifier : byte
		{
			None,
			Numeric,
			Operator,
			String,
			Space,
			Structure
		}

		[StructLayout (LayoutKind.Explicit)]
		public struct Token
		{
			[FieldOffset (0)] public OperationCode Operation;
			[FieldOffset (1)] public MathValue Value;
			[FieldOffset (1)] public byte Source;
			[FieldOffset (2)] public sbyte Multiplier;

			public static Token None => new Token () { Operation = OperationCode.None };

			public MathValue ValueWithSources (IValueProvider[] sources)
			{
				if (Operation == OperationCode.Source)
				{
					var sourceValue = sources[Source].Value;
					switch (sourceValue.ValueClass)
					{
						case ValueClassifier.Numeric: return new MathValue (sourceValue.FloatValue * Multiplier, false);
						default: return sourceValue;
					}
				}
				return Value;
			}

			public bool IsOperator => Operation == OperationCode.Plus ||
						Operation == OperationCode.Minus ||
						Operation == OperationCode.Multiply ||
						Operation == OperationCode.Divide ||
						Operation == OperationCode.Power;

			public static Token Operator (OperationCode operation)
			{
				return new Token ()
				{
					Operation = operation
				};
			}

			public static Token ReadSource (byte sourceId, bool negative)
			{
				return new Token ()
				{
					Operation = OperationCode.Source,
					Source = sourceId,
					Multiplier = negative ? (sbyte)-1 : (sbyte)1
				};
			}

			public static Token StaticValue (MathValue value)
			{
				return new Token ()
				{
					Operation = OperationCode.Value,
					Value = value
				};
			}

			public override string ToString ()
			{
				switch (Operation)
				{
					case OperationCode.OpenParentheses: return "(";
					case OperationCode.CloseParentheses: return ")";

					case OperationCode.Plus: return "+";
					case OperationCode.Minus: return "-";
					case OperationCode.Multiply: return "*";
					case OperationCode.Divide: return "/";
					case OperationCode.Power: return "^";
					case OperationCode.Percentage: return "%";

					case OperationCode.And: return "&&";
					case OperationCode.Or: return "||";
					case OperationCode.Equal: return "==";
					case OperationCode.NotEqual: return "!=";
					case OperationCode.GreaterThan: return ">";
					case OperationCode.GreaterThanOrEqual: return ">=";
					case OperationCode.LessThan: return "<";
					case OperationCode.LessThanOrEqual: return "<=";

					case OperationCode.Value: return Value.ToString ();
					case OperationCode.Source: return "source[" + Source.ToString () + "]";
					default: return Operation.ToString ();
				}
			}
		}

		struct CalculatedSpan
		{
			public static CalculatedSpan None => new CalculatedSpan ();

			public int Start;
			public int Length;
			public MathValue Value;

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

		private readonly Token[] Tokens;
		private readonly IValueProvider[] Sources;
		private readonly IValueProvider ImplicitSource;
		private readonly List<CalculatedSpan> Buffer;

		/// <summary>
		/// <para></para>
		/// </summary>
		public string Expression { get; private set; }

		/// <summary>
		/// <para></para>
		/// </summary>
		public IMathContext Context { get; private set; }

		/// <summary>
		/// <para></para>
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="context"></param>
		public CompiledExpression (string expression, IMathContext context = null)
		{
			Expression = expression;
			Context = context;

			Buffer = new List<CalculatedSpan> ();

			ParseText (expression, out Tokens, out ImplicitSource, out Sources, context);
		}

		/// <summary>
		/// <para></para>
		/// </summary>
		public MathValue Evaluate ()
		{
			return Evaluate (Tokens);
		}

		public override string ToString ()
		{
			var sb = new StringBuilder ();

			var lastToken = Tokens[0];
			for (int i = 1; i < Tokens.Length; i++)
			{
				var token = Tokens[i];
				sb.Append (lastToken);
				if (lastToken.Operation != OperationCode.OpenParentheses
					&& token.Operation != OperationCode.CloseParentheses
					&& token.Operation != OperationCode.Percentage)
				{
					sb.Append (' ');
				}
				lastToken = token;
			}
			sb.Append (Tokens[Tokens.Length - 1]);
			return sb.ToString ();
		}

		private struct CharacterDescriptor
		{
			private readonly string SourceString;
			public short Index;
			public char Character;

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

			public static CharacterDescriptor Start (string sourceString)
			{
				return new CharacterDescriptor (sourceString, -1);
			}

			public static CharacterDescriptor End (string sourceString)
			{
				return new CharacterDescriptor (sourceString, -2);
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

			public CharacterDescriptor Next ()
			{
				if (Index == -2)
					return this;
				return new CharacterDescriptor (SourceString, (short)(Index + 1));
			}

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

			public override string ToString ()
			{
				if (Index == -1)
					return "Start";
				if (Index == -2)
					return "End";

				return $"\"{Character}\" at {Index}";
			}
		}

		private static void ParseText (string expression, out Token[] tokens, out IValueProvider implicitSource, out IValueProvider[] sources, IMathContext context = null)
		{
			var foundTokens = new List<Token> ();
			var foundSources = new List<IValueProvider> ();

			int currentSpanStart = 0;
			int currentSpanLength = 0;
			var currentSpanClass = SpanClassifier.None;
			var characterClass = SpanClassifier.None;

			var lastToken = Token.None;

			var headCharacter = CharacterDescriptor.Start (expression);
			var lastCharacter = headCharacter;

			while (true)
			{
				headCharacter = headCharacter.Next ();

				if (headCharacter.Index == -2 && lastCharacter.Index == -2)
					break;

				characterClass = headCharacter.Type;

				if (headCharacter.Character == '-' && (lastToken.IsOperator || lastToken.Operation == OperationCode.None))
				{
					characterClass = SpanClassifier.Numeric;
				}

				if (characterClass != currentSpanClass || characterClass == SpanClassifier.Structure)
				{
					if (currentSpanClass == SpanClassifier.None && characterClass != SpanClassifier.Structure
						|| currentSpanClass == SpanClassifier.Numeric
						&& currentSpanLength == 1
						&& lastCharacter.Character == '-')
					{
						currentSpanClass = characterClass;
					}
					else
					{
						string debug = expression.Substring (currentSpanStart, currentSpanLength);
						switch (currentSpanClass)
						{
							case SpanClassifier.String:

								bool isNegative = expression[currentSpanStart] == '-';
								int offset = isNegative ? 1 : 0;
#if USE_SPANS
								var spanContent = expression.AsSpan().Slice(currentSpanStart, currentSpanLength));
#else
								string spanContent = expression.Substring (currentSpanStart + offset, currentSpanLength - offset);
#endif

								if (spanContent.Equals("true", StringComparison.OrdinalIgnoreCase))
								{
									lastToken = Token.StaticValue (new MathValue (true));
									foundTokens.Add (lastToken);
								}
								else if (spanContent.Equals("false", StringComparison.OrdinalIgnoreCase))
								{
									lastToken = Token.StaticValue (new MathValue (false));
									foundTokens.Add (lastToken);
								}
								else if (context == null)
								{
									throw new InvalidOperationException (string.Format ("Unrecognised source \"{0}\" at position {1} to {2} (No additional sources specified)",
										spanContent, currentSpanStart, currentSpanStart + currentSpanLength));
								}
								else
								{
									IValueProvider provider;
									if (!context.TryGetTerm (spanContent, out provider))
									{
										throw new InvalidOperationException (string.Format ("Unrecognised source \"{0}\" at position {1} to {2}",
											spanContent, currentSpanStart, currentSpanStart + currentSpanLength));
									}

									int sourceIndex = foundSources.IndexOf (provider);
									if (sourceIndex == -1)
									{
										sourceIndex = foundSources.Count;
										foundSources.Add (provider);
									}

									lastToken = Token.ReadSource ((byte)sourceIndex, isNegative);
									foundTokens.Add (lastToken);
								}
								break;

							case SpanClassifier.Numeric:
#if USE_SPANS
								spanContent = expression.AsSpan().Slice(currentSpanStart, currentSpanLength));
#else
								spanContent = expression.Substring (currentSpanStart, currentSpanLength);
#endif

								float numericFloat = float.Parse (spanContent);

								lastToken = Token.StaticValue (new MathValue (numericFloat, false));
								foundTokens.Add (lastToken);

								break;

							case SpanClassifier.Operator:
							case SpanClassifier.Structure:
								if (currentSpanLength == 1)
								{
									lastToken = Token.Operator (lastCharacter.OperationCode);
								}
								else
								{
									spanContent = expression.Substring (currentSpanStart, currentSpanLength);

									OperationCode operation;
									switch (spanContent)
									{
										case "==": operation = OperationCode.Equal; break;
										case "!=": operation = OperationCode.NotEqual; break;
										case ">=": operation = OperationCode.GreaterThanOrEqual; break;
										case "<=": operation = OperationCode.LessThanOrEqual; break;
										case "&&": operation = OperationCode.And; break;
										case "||": operation = OperationCode.Or; break;
										default: operation = OperationCode.None; break;
									}
									lastToken = Token.Operator (operation);
								}
								if (lastToken.Operation == OperationCode.None)
								{
									throw new InvalidOperationException (string.Format ("Unrecognised Operator Sequence \"{0}\"", expression.Substring (currentSpanStart, currentSpanLength)));
								}
								foundTokens.Add (lastToken);
								break;
						}

						currentSpanStart = headCharacter.Index;
						currentSpanLength = 0;
						currentSpanClass = characterClass;
					}
				}

				currentSpanLength++;

				lastCharacter = headCharacter;
			}

			tokens = foundTokens.ToArray ();
			sources = foundSources.ToArray ();
			implicitSource = null;
		}

		MathValue Evaluate (Token[] tokens)
		{
			Buffer.Clear ();
			return Evaluate (tokens, 0, tokens.Length);
		}

		struct IndexDescriptor
		{
			public MathValue Value;
			public Token Token;

			public IndexDescriptor (MathValue value, Token token)
			{
				Value = value;
				Token = token;
			}
		}

		MathValue Evaluate (Token[] tokens, int spanStart, int spanLength)
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
				&& finalSpan.Start + finalSpan.Length < tokens.Length
				&& finalSpan.Start + finalSpan.Length < tokens.Length)
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

		IndexDescriptor DescribeIndex (Token[] tokens, int index)
		{
			for (int j = 0; j < Buffer.Count; j++)
			{
				var span = Buffer[j];
				if (span.Contains (index))
				{
					return new IndexDescriptor (span.Value, Token.None);
				}
			}

			var token = tokens[index];
			return new IndexDescriptor (token.ValueWithSources (Sources), token);
		}

		IndexDescriptor DescribeIndex (Token[] tokens, int index, out CalculatedSpan span, out int spanIndex)
		{
			for (int j = 0; j < Buffer.Count; j++)
			{
				span = Buffer[j];
				if (span.Contains (index))
				{
					spanIndex = j;
					return new IndexDescriptor (span.Value, Token.None);
				}
			}
			spanIndex = -1;
			span = CalculatedSpan.None;

			var token = tokens[index];
			return new IndexDescriptor (token.ValueWithSources (Sources), token);
		}
	}
}
