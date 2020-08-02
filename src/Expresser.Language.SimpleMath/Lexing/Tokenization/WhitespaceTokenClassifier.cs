using Expresser.Lexing.Common;

namespace Expresser.Language.SimpleMath.Lexing.Tokenization
{
	public class WhitespaceTokenClassifier : CharacterCategoryTokenClassifier
	{
		public override bool IsMatched(char character)
		{
			return char.IsWhiteSpace(character);
		}
	}
}
