namespace Expresser.Syntax
{
	public interface ITokenClassifier
	{
		NextCharacterResult NextCharacter(char nextCharacter);
		void Reset();
	}
}
