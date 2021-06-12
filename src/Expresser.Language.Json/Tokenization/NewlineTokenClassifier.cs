using Expresser.Lexing;

namespace Expresser.Language.Json.Tokenization
{
	internal class NewlineTokenClassifier : ITokenClassifier
	{
		/// <inheritdoc/>
		public void Reset()
		{
		}

		/// <inheritdoc/>
		public ClassifierAction NextCharacter(char nextCharacter)
		{
			return nextCharacter switch
			{
				'\r' => ClassifierAction.ContinueReading(),
				'\n' => ClassifierAction.TokenizeImmediately(),
				_ => ClassifierAction.GiveUp()
			};
		}
	}
}
