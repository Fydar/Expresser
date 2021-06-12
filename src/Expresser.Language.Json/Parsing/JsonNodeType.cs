namespace Expresser.Language.Json.Parsing
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
