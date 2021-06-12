using Expresser.Lexing;

namespace Expresser.Language.SimpleMath.Lexing.Tokenization
{
	public class MultiLineCommentTokenClassifier : ITokenClassifier
	{
		private int modeIndex = 0;

		public void Reset()
		{
			modeIndex = 0;
		}

		public ClassifierAction NextCharacter(char nextCharacter)
		{
			if (modeIndex == 0)
			{
				modeIndex = 1;
				if (nextCharacter == '/')
				{
					return ClassifierAction.ContinueReading();
				}
				else
				{
					return ClassifierAction.GiveUp();
				}
			}
			else if (modeIndex == 1)
			{
				modeIndex = 2;
				if (nextCharacter == '*')
				{
					return ClassifierAction.ContinueReading();
				}
				else
				{
					return ClassifierAction.GiveUp();
				}
			}
			else if (modeIndex == 2)
			{
				if (nextCharacter == '*')
				{
					modeIndex = 3;
					return ClassifierAction.ContinueReading();
				}
				else
				{
					return ClassifierAction.ContinueReading();
				}
			}
			else
			{
				if (nextCharacter == '/')
				{
					return ClassifierAction.TokenizeImmediately();
				}
				else
				{
					modeIndex = 2;
					return ClassifierAction.ContinueReading();
				}
			}
		}
	}
}
