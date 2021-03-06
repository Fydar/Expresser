﻿using Expresser.Lexing;

namespace Expresser.Language.SimpleMath.Lexing.Tokenization
{
	internal class StringTokenClassifier : ITokenClassifier
	{
		private bool isFirstCharacter = true;
		private bool isEscaped = false;

		/// <inheritdoc/>
		public void Reset()
		{
			isFirstCharacter = true;
		}

		/// <inheritdoc/>
		public ClassifierAction NextCharacter(char nextCharacter)
		{
			if (isFirstCharacter)
			{
				isFirstCharacter = false;

				if (nextCharacter == '"')
				{
					return ClassifierAction.ContinueReading();
				}
				else
				{
					return ClassifierAction.GiveUp();
				}
			}
			else
			{
				if (nextCharacter == '\\')
				{
					isEscaped = true;
					return ClassifierAction.ContinueReading();
				}
				else if (nextCharacter == '"')
				{
					if (isEscaped)
					{
						isEscaped = false;
						return ClassifierAction.ContinueReading();
					}
					else
					{
						return ClassifierAction.TokenizeImmediately();
					}
				}
				else
				{
					return ClassifierAction.ContinueReading();
				}
			}
		}
	}
}
