using Expresser.Language.SimpleMath.Lexing.Tokenization;
using Expresser.Lexing;
using System;

namespace Expresser.Language.SimpleMath.Lexing
{
	public class SimpleMathLexerLanguage : ILexerLanguage
	{
		public static Lexer Lexer { get; } = new Lexer(new SimpleMathLexerLanguage());

		/// <inheritdoc/>
		public ITokenClassifier[] Classifiers { get; } = new ITokenClassifier[]
		{
			new KeywordTokenClassifier("true"),
			new KeywordTokenClassifier("false"),

			new NumericTokenClassifier(),
			new IdentifierTokenClassifier(),
			new WhitespaceTokenClassifier(),

			new SingleCharacterTokenClassifier('('),
			new SingleCharacterTokenClassifier(')'),

			new MultiCharacterOpTokenClassifier("=="),
			new MultiCharacterOpTokenClassifier("!="),
			new MultiCharacterOpTokenClassifier("<="),
			new MultiCharacterOpTokenClassifier(">="),
			new SingleCharacterTokenClassifier('>'),
			new SingleCharacterTokenClassifier('<'),

			new SingleCharacterTokenClassifier('+'),
			new SingleCharacterTokenClassifier('-'),
			new SingleCharacterTokenClassifier('*'),
			new SingleCharacterTokenClassifier('/'),
			new SingleCharacterTokenClassifier('^'),
			new SingleCharacterTokenClassifier('&'),
			new SingleCharacterTokenClassifier('|'),
			new SingleCharacterTokenClassifier('!'),
		};

		/// <inheritdoc/>
		public ConsoleColor[] Colors { get; } = new ConsoleColor[]
		{
			ConsoleColor.Blue,
			ConsoleColor.Blue,

			ConsoleColor.Yellow,
			ConsoleColor.Gray,
			ConsoleColor.Red,

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
		};
	}
}
