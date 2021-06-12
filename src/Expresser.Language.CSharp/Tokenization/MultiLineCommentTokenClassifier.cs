using Expresser.Lexing;

namespace Expresser.Language.CSharp.Tokenization
{
	internal class MultiLineCommentTokenClassifier : ITokenClassifier
	{
		private enum State : byte
		{
			ExpectingStartingSlash,
			ExpectingStartingAsterisk,
			ExpectingEndingAsterisk,
			ExpectingEndingSlash,
		}

		private State state;

		/// <inheritdoc/>
		public void Reset()
		{
			state = State.ExpectingStartingSlash;
		}

		/// <inheritdoc/>
		public ClassifierAction NextCharacter(char nextCharacter)
		{
			switch (state)
			{
				case State.ExpectingStartingSlash:
				{
					state = State.ExpectingStartingAsterisk;
					return nextCharacter == '/'
						? ClassifierAction.ContinueReading()
						: ClassifierAction.GiveUp();
				}
				case State.ExpectingStartingAsterisk:
				{
					state = State.ExpectingEndingAsterisk;
					return nextCharacter == '*'
						? ClassifierAction.ContinueReading()
						: ClassifierAction.GiveUp();
				}
				case State.ExpectingEndingAsterisk:
				{
					if (nextCharacter == '*')
					{
						state = State.ExpectingEndingSlash;
						return ClassifierAction.ContinueReading();
					}
					else
					{
						return ClassifierAction.ContinueReading();
					}
				}
				default:
				{
					if (nextCharacter == '/')
					{
						return ClassifierAction.TokenizeImmediately();
					}
					else
					{
						state = State.ExpectingStartingAsterisk;
						return ClassifierAction.ContinueReading();
					}
				}
			}
		}
	}
}
