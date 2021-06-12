using Expresser.Lexing.Common;

namespace Expresser.Language.SimpleMath.Lexing.Tokenization
{
	internal class WhitespaceTokenClassifier : CharacterCategoryTokenClassifier
	{
		public override bool IsMatched(char character)
		{
			return char.IsWhiteSpace(character);
		}
	}
}
