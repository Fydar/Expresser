using Expresser.Syntax;

namespace Expresser.Demo.Json.Tokenization
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
						Action = ClassificationAction.ContinueReading,
					};
				}
				else
				{
					return new NextCharacterResult()
					{
						Action = ClassificationAction.GiveUp
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
						Action = ClassificationAction.ContinueReading,
					};
				}
				else
				{
					return new NextCharacterResult()
					{
						Action = ClassificationAction.GiveUp
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
						Action = ClassificationAction.ContinueReading,
					};
				}
				else
				{
					return new NextCharacterResult()
					{
						Action = ClassificationAction.ContinueReading,
					};
				}
			}
			else
			{
				if (nextCharacter == '/')
				{
					return new NextCharacterResult()
					{
						Action = ClassificationAction.TokenizeImmediately,
					};
				}
				else
				{
					modeIndex = 2;
					return new NextCharacterResult()
					{
						Action = ClassificationAction.ContinueReading,
					};
				}
			}
		}
	}
}
