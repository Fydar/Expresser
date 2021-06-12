using Expresser.Language.CSharp.Tokenization;
using Expresser.Lexing;
using System;

namespace Expresser.Language.CSharp
{
	public class CSharpLexerLanguage : ILexerLanguage
	{
		/// <inheritdoc/>
		public ITokenClassifier[] Classifiers { get; } = new ITokenClassifier[]
		{
			new LineCommentTokenClassifier(),
			new MultiLineCommentTokenClassifier(),
			new StringTokenClassifier(),
			new CharacterLiteralTokenClassifier(),

			new KeywordTokenClassifier("void"),
			new KeywordTokenClassifier("bool"),
			new KeywordTokenClassifier("byte"),
			new KeywordTokenClassifier("char"),
			new KeywordTokenClassifier("decimal"),
			new KeywordTokenClassifier("double"),
			new KeywordTokenClassifier("float"),
			new KeywordTokenClassifier("int"),
			new KeywordTokenClassifier("long"),
			new KeywordTokenClassifier("sbyte"),
			new KeywordTokenClassifier("short"),
			new KeywordTokenClassifier("string"),
			new KeywordTokenClassifier("uint"),
			new KeywordTokenClassifier("ulong"),
			new KeywordTokenClassifier("ushort"),

			new KeywordTokenClassifier("var"),
			new KeywordTokenClassifier("using"),
			new KeywordTokenClassifier("public"),
			new KeywordTokenClassifier("private"),
			new KeywordTokenClassifier("static"),
			new KeywordTokenClassifier("new"),

			new KeywordTokenClassifier("in"),
			new KeywordTokenClassifier("namespace"),
			new KeywordTokenClassifier("class"),
			new KeywordTokenClassifier("struct"),

			new KeywordTokenClassifier("foreach"),
			new KeywordTokenClassifier("for"),
			new KeywordTokenClassifier("if"),
			new KeywordTokenClassifier("else"),

			new KeywordTokenClassifier("true"),
			new KeywordTokenClassifier("false"),
			new KeywordTokenClassifier("null"),

			new MultiCharacterOpTokenClassifier("=="),
			new MultiCharacterOpTokenClassifier("!="),
			new MultiCharacterOpTokenClassifier("<="),
			new MultiCharacterOpTokenClassifier(">="),
			new SingleCharacterTokenClassifier('>'),
			new SingleCharacterTokenClassifier('<'),

			new MultiCharacterOpTokenClassifier("--"),
			new MultiCharacterOpTokenClassifier("-="),
			new MultiCharacterOpTokenClassifier("++"),
			new MultiCharacterOpTokenClassifier("+="),

			new SingleCharacterTokenClassifier('+'),
			new SingleCharacterTokenClassifier('-'),
			new SingleCharacterTokenClassifier('*'),
			new SingleCharacterTokenClassifier('/'),
			new SingleCharacterTokenClassifier('^'),
			new SingleCharacterTokenClassifier('='),
			new SingleCharacterTokenClassifier('?'),
			new SingleCharacterTokenClassifier('!'),

			new SingleCharacterTokenClassifier('('),
			new SingleCharacterTokenClassifier(')'),
			new SingleCharacterTokenClassifier('{'),
			new SingleCharacterTokenClassifier('}'),
			new SingleCharacterTokenClassifier('['),
			new SingleCharacterTokenClassifier(']'),
			new SingleCharacterTokenClassifier(','),
			new SingleCharacterTokenClassifier(':'),
			new SingleCharacterTokenClassifier(';'),
			new SingleCharacterTokenClassifier('.'),

			new NumericTokenClassifier(),
			new IdentifierTokenClassifier(),
			new WhitespaceTokenClassifier(),
		};

		/// <inheritdoc/>
		public ConsoleColor[] Colors { get; } = new ConsoleColor[]
		{
			ConsoleColor.Green,
			ConsoleColor.Green,
			ConsoleColor.Yellow,
			ConsoleColor.Yellow,

			ConsoleColor.Blue,
			ConsoleColor.Blue,
			ConsoleColor.Blue,
			ConsoleColor.Blue,
			ConsoleColor.Blue,
			ConsoleColor.Blue,
			ConsoleColor.Blue,
			ConsoleColor.Blue,
			ConsoleColor.Blue,
			ConsoleColor.Blue,
			ConsoleColor.Blue,
			ConsoleColor.Blue,
			ConsoleColor.Blue,
			ConsoleColor.Blue,
			ConsoleColor.Blue,

			ConsoleColor.Blue,
			ConsoleColor.Blue,
			ConsoleColor.Blue,
			ConsoleColor.Blue,
			ConsoleColor.Blue,
			ConsoleColor.Blue,

			ConsoleColor.Blue,
			ConsoleColor.Blue,
			ConsoleColor.Blue,
			ConsoleColor.Blue,

			ConsoleColor.Magenta,
			ConsoleColor.Magenta,
			ConsoleColor.Magenta,
			ConsoleColor.Magenta,

			ConsoleColor.Blue,
			ConsoleColor.Blue,
			ConsoleColor.Blue,

			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,

			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,

			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,

			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,


			ConsoleColor.Yellow,
			ConsoleColor.White,
			ConsoleColor.Red,
		};
	}
}
