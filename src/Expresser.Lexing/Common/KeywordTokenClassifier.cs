﻿namespace Expresser.Lexing.Common
{
	public class KeywordTokenClassifier : ITokenClassifier
	{
		private int currentIndex = 0;

		public string Keyword { get; }

		public KeywordTokenClassifier(string keyword)
		{
			Keyword = keyword;
		}

		/// <inheritdoc/>
		public void Reset()
		{
			currentIndex = 0;
		}

		/// <inheritdoc/>
		public ClassifierAction NextCharacter(char nextCharacter)
		{
			string keyword = Keyword;

			if (currentIndex == keyword.Length)
			{
				if (char.IsLetterOrDigit(nextCharacter))
				{
					return ClassifierAction.GiveUp();
				}
				else
				{
					return ClassifierAction.TokenizeFromLast();
				}
			}
			else
			{
				if (nextCharacter == keyword[currentIndex])
				{
					currentIndex++;
					return ClassifierAction.ContinueReading();
				}
				else
				{
					return ClassifierAction.GiveUp();
				}
			}
		}
	}
}
