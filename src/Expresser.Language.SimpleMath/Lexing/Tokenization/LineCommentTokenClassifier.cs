using Expresser.Lexing;

namespace Expresser.Language.SimpleMath.Lexing.Tokenization
{
	internal class LineCommentTokenClassifier : ITokenClassifier
	{
		private int characterIndex = 0;

		/// <inheritdoc/>
		public void Reset()
		{
			characterIndex = 0;
		}

		/// <inheritdoc/>
		public ClassifierAction NextCharacter(char nextCharacter)
		{
			if (characterIndex == 0)
			{
				characterIndex++;
				if (nextCharacter == '/')
				{
					return ClassifierAction.ContinueReading();
				}
				else
				{
					return ClassifierAction.GiveUp();
				}
			}
			if (characterIndex == 1)
			{
				characterIndex++;
				if (nextCharacter == '/')
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
				characterIndex++;
				if (nextCharacter == '\n')
				{
					return ClassifierAction.TokenizeImmediately();
				}
				else
				{
					return ClassifierAction.ContinueReading();
				}
			}
		}
	}
}
