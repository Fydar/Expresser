using Expresser.Demo.Json.Tokenization;
using Expresser.Syntax;
using System;

namespace Expresser.Demo.Json
{
	public class JsonLang : ILanguage
	{
		public ITokenClassifier[] Classifiers { get; } = new ITokenClassifier[]
		{
				new StringTokenClassifier(),
				new NumericTokenClassifier(),

				new WhitespaceTokenClassifier(),
				new MultiLineCommentTokenClassifier(),

				new SingleCharacterTokeniser('{'),
				new SingleCharacterTokeniser('}'),
				new SingleCharacterTokeniser('['),
				new SingleCharacterTokeniser(']'),
				new SingleCharacterTokeniser(','),
				new SingleCharacterTokeniser(':'),
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
		};
	}
}
