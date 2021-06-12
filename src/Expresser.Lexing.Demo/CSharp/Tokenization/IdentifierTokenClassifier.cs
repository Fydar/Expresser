namespace Expresser.Lexing.Demo.CSharp.Tokenization
{
	internal class IdentifierTokenClassifier : CharacterCategoryTokenClassifier
	{
		public override bool IsMatched(char character)
		{
			return (!IsFirstCharacter || !char.IsDigit(character)) && char.IsLetterOrDigit(character);
		}
	}
}
