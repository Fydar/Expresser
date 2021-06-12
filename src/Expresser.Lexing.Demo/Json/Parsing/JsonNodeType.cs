namespace Expresser.Lexing.Demo.Json
{
	public enum JsonNodeType
	{
		StartObject,
		EndObject,
		StartArray,
		EndArray,
		PropertyName,
		PropertyValueDeliminator,
		SingleLineComment,
		MultiLineComment,
		ValueDeliminator,
		StringLiteral,
		NumberLiteral,
		TrueLiteral,
		FalseLiteral,
		NullLiteral,
		Whitespace,
		Newline,
	}
}
