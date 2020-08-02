using System;

namespace Expresser.Lexing
{
	public interface ILexerLanguage
	{
		ITokenClassifier[] Classifiers { get; }
		ConsoleColor[] Colors { get; }
	}
}
