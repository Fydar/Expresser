using Expresser.Syntax;

namespace Expresser.Demo.CSharp.Tokenization
{
	public class LineCommentTokenClassifier : ITokenClassifier
	{
		private int characterIndex = 0;

		public void Reset()
		{
			characterIndex = 0;
		}

		public NextCharacterResult NextCharacter(char nextCharacter)
		{
			if (characterIndex == 0)
			{
				characterIndex++;
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
			if (characterIndex == 1)
			{
				characterIndex++;
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
			else
			{
				characterIndex++;
				if (nextCharacter == '\n')
				{
					return new NextCharacterResult()
					{
						Action = ClassificationAction.TokenizeImmediately,
					};
				}
				else
				{
					return new NextCharacterResult()
					{
						Action = ClassificationAction.ContinueReading
					};
				}
			}
		}
	}
}
