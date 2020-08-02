using Expresser.Lexing;

namespace Expresser.Demo.CSharp.Tokenization
{
	public class MultiLineCommentTokenClassifier : ITokenClassifier
	{
		private int modeIndex = 0;

		public void Reset()
		{
			modeIndex = 0;
		}

		public NextCharacterResult NextCharacter(char nextCharacter)
		{
			if (modeIndex == 0)
			{
				modeIndex = 1;
				if (nextCharacter == '/')
				{
					return new NextCharacterResult()
					{
						Action = ClassifierAction.ContinueReading,
					};
				}
				else
				{
					return new NextCharacterResult()
					{
						Action = ClassifierAction.GiveUp
					};
				}
			}
			else if (modeIndex == 1)
			{
				modeIndex = 2;
				if (nextCharacter == '*')
				{
					return new NextCharacterResult()
					{
						Action = ClassifierAction.ContinueReading,
					};
				}
				else
				{
					return new NextCharacterResult()
					{
						Action = ClassifierAction.GiveUp
					};
				}
			}
			else if (modeIndex == 2)
			{
				if (nextCharacter == '*')
				{
					modeIndex = 3;
					return new NextCharacterResult()
					{
						Action = ClassifierAction.ContinueReading,
					};
				}
				else
				{
					return new NextCharacterResult()
					{
						Action = ClassifierAction.ContinueReading,
					};
				}
			}
			else
			{
				if (nextCharacter == '/')
				{
					return new NextCharacterResult()
					{
						Action = ClassifierAction.TokenizeImmediately,
					};
				}
				else
				{
					modeIndex = 2;
					return new NextCharacterResult()
					{
						Action = ClassifierAction.ContinueReading,
					};
				}
			}
		}
	}
}
