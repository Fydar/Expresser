using Expresser.Demo.Json.Tokenization;
using Expresser.Lexing;
using Expresser.Lexing.Common;
using System;

namespace Expresser.Demo.Json
{
	public class JsonLang : ILexerLanguage
	{
		public ITokenClassifier[] Classifiers { get; } = new ITokenClassifier[]
		{
			new StringTokenClassifier(),
			new NumericTokenClassifier(),

			new WhitespaceTokenClassifier(),
			new MultiLineCommentTokenClassifier(),

			new SingleCharacterTokenClassifier('{'),
			new SingleCharacterTokenClassifier('}'),
			new SingleCharacterTokenClassifier('['),
			new SingleCharacterTokenClassifier(']'),
			new SingleCharacterTokenClassifier(','),
			new SingleCharacterTokenClassifier(':'),

			new KeywordTokenClassifier("null"),
			new KeywordTokenClassifier("true"),
			new KeywordTokenClassifier("false"),
		};

		public ConsoleColor[] Colors { get; } = new ConsoleColor[]
		{
			ConsoleColor.Yellow,
			ConsoleColor.Blue,

			ConsoleColor.Red,
			ConsoleColor.Green,

			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,
			ConsoleColor.DarkGray,

			ConsoleColor.DarkMagenta,
			ConsoleColor.DarkMagenta,
			ConsoleColor.DarkMagenta,
		};
	}
}
