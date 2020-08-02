using System;

namespace Expresser.Syntax
{
	public interface ILanguage
	{
		ITokenClassifier[] Classifiers { get; }
		ConsoleColor[] Colors { get; }
	}
}
