using Expresser.Lexing;

namespace Expresser.Language.CSharp.Tokenization
{
	internal class CharacterLiteralTokenClassifier : ITokenClassifier
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

				return nextCharacter == '\''
					? ClassifierAction.ContinueReading()
					: ClassifierAction.GiveUp();
			}
			else
			{
				if (nextCharacter == '\\')
				{
					isEscaped = true;
					return ClassifierAction.ContinueReading();
				}
				else if (nextCharacter == '\'')
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
