﻿using Expresser.Language.Json.Tokenization;
using Expresser.Lexing;
using System;

namespace Expresser.Language.Json
{
	public class JsonLexerLanguage : ILexerLanguage
	{
		/// <inheritdoc/>
		public ITokenClassifier[] Classifiers { get; } = new ITokenClassifier[]
		{
			new StringTokenClassifier(),
			new NumericTokenClassifier(),

			new WhitespaceTokenClassifier(),
			new NewlineTokenClassifier(),
			new MultiLineCommentTokenClassifier(),
			new LineCommentTokenClassifier(),

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

		/// <inheritdoc/>
		public ConsoleColor[] Colors { get; } = new ConsoleColor[]
		{
			ConsoleColor.Yellow,
			ConsoleColor.Blue,

			ConsoleColor.Red,
			ConsoleColor.Red,
			ConsoleColor.Green,
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
