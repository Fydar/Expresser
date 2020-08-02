using Expresser.Syntax;

namespace Expresser.Demo.CSharp.Tokenization
{
	public class IdentifierTokenClassifier : CharacterCategoryClassifier
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
