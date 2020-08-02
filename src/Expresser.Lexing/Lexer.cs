using System.Collections.Generic;

namespace Expresser.Lexing
{
	public class Lexer
	{
		private struct ClassifierState
		{
			public bool HasGivenUp;
			public int EndIndex;
		}

		public ILexerLanguage LexerLanguage { get; }

		public Lexer(ILexerLanguage lexerLanguage)
		{
			LexerLanguage = lexerLanguage;
		}

		public IEnumerable<LexerToken> Evaluate(string source)
		{
			var classifiers = LexerLanguage.Classifiers;

			int classifiersCount = classifiers.Length;
			var classifierStates = new ClassifierState[classifiersCount];

			for (int i = 0; i < classifiersCount; i++)
			{
				var classifier = classifiers[i];
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

					var classifier = classifiers[i];
					var result = classifier.NextCharacter(c);
					if (result.Action == ClassifierAction.GiveUp)
					{
						classifierState.HasGivenUp = true;
						classifierStates[i] = classifierState;
					}
					else if (result.Action == ClassifierAction.TokenizeFromLast)
					{
						classifierState.EndIndex = charIndex - 1;
						classifierStates[i] = classifierState;
					}
					else if (result.Action == ClassifierAction.TokenizeImmediately)
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
					int longest = -1;
					for (int i = 0; i < classifiersCount; i++)
					{
						var classifierState = classifierStates[i];

						if (classifierState.EndIndex > longest)
						{
							longest = classifierState.EndIndex;
						}
					}

					bool tokenized = false;
					for (int i = 0; i < classifiersCount; i++)
					{
						var classifierState = classifierStates[i];

						if (classifierState.EndIndex != -1 && 
							classifierState.EndIndex == longest)
						{
							int classifierEndIndex = classifierState.EndIndex;

							yield return new LexerToken(
								lineNumber: 0,
								startIndex: startIndex,
								length: classifierEndIndex - startIndex + 1,
								classifierIndex: i
							);

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
							yield return new LexerToken(
								lineNumber: 0,
								startIndex: startIndex,
								length: charIndex - startIndex + 1,
								classifierIndex: -1
							);
						}

						startIndex += 1;
					}

					for (int i = 0; i < classifiersCount; i++)
					{
						var classifier = classifiers[i];
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
