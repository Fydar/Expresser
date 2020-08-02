using Expresser.Lexing.Common;

namespace Expresser.Language.SimpleMath.Lexing.Tokenization
{
	public class IdentifierTokenClassifier : CharacterCategoryTokenClassifier
	{
		public override bool IsMatched(char character)
		{
			if (IsFirstCharacter
				&& char.IsDigit(character))
			{
				return false;
			}

			return char.IsLetterOrDigit(character);
		}
	}
}
