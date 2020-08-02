using System;
using System.Threading;

namespace Expresser.Syntax
{
	public class Lexer
	{
		public ILanguage Language { get; }
	}

	public static class ConsoleHighlighter
	{
		struct ClassifierState
		{
			public bool HasGivenUp;
			public int EndIndex;
		}

		public static void Highlight(string source, ILanguage language)
		{
			int classifiersCount = language.Classifiers.Length;
			var classifierStates = new ClassifierState[classifiersCount];

			for (int i = 0; i < classifiersCount; i++)
			{
				var classifier = language.Classifiers[i];
				classifierStates[i] = new ClassifierState()
				{
					EndIndex = -1
				};
				classifier.Reset();
			}

			int startIndex = 0;
			for (int charIndex = 0; charIndex < source.Length + 1; charIndex++)
			{
				char c = charIndex != source.Length
					? source[charIndex]
					: ' ';

				bool anyContinuing = false;
				for (int i = 0; i < classifiersCount; i++)
				{
					var classifierState = classifierStates[i];

					if (classifierState.HasGivenUp)
					{
						continue;
					}

					var classifier = language.Classifiers[i];
					var result = classifier.NextCharacter(c);
					if (result.Action == ClassificationAction.GiveUp)
					{
						classifierState.HasGivenUp = true;
						classifierStates[i] = classifierState;
					}
					else if (result.Action == ClassificationAction.TokenizeFromLast)
					{
						classifierState.EndIndex = charIndex - 1;
						classifierStates[i] = classifierState;
					}
					else if (result.Action == ClassificationAction.TokenizeImmediately)
					{
						classifierState.EndIndex = charIndex;
						classifierStates[i] = classifierState;
					}
					else // if (result.Action == ClassificationAction.ContinueReading)
					{
						anyContinuing = true;
					}
				}
				if (!anyContinuing)
				{
					bool tokenized = false;
					for (int i = 0; i < classifiersCount; i++)
					{
						var classifierState = classifierStates[i];

						if (classifierState.EndIndex != -1)
						{
							int classifierEndIndex = classifierState.EndIndex;

							Console.ForegroundColor = language.Colors[i];
							Console.Write(source.Substring(startIndex, classifierEndIndex - startIndex + 1));
							Thread.Sleep(10);

							// Console.WriteLine($"{classifier.GetType().Name}: \"{source.Substring(startIndex, classifierEndIndex - startIndex + 1)}\"");
							startIndex = classifierEndIndex + 1;
							charIndex = classifierEndIndex;
							tokenized = true;
							break;
						}
					}
					if (!tokenized)
					{
						if (charIndex != source.Length)
						{
							// Console.WriteLine($"Unrecognised string in sequence: \"{source.Substring(startIndex, charIndex - startIndex + 1)}\"");

							Console.ForegroundColor = ConsoleColor.Red;
							Console.Write(source[startIndex]);
							Thread.Sleep(10);
						}

						startIndex += 1;
					}

					for (int i = 0; i < classifiersCount; i++)
					{
						var classifier = language.Classifiers[i];
						classifierStates[i] = new ClassifierState()
						{
							EndIndex = -1
						};
						classifier.Reset();
					}
				}
			}
		}
	}
}
