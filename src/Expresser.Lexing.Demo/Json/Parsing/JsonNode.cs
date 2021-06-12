﻿namespace Expresser.Lexing.Demo.Json
{
	public readonly struct JsonNode
	{
		public JsonNodeType Type { get; }
		public int StartIndex { get; }
		public int Length { get; }

		public JsonNode(JsonNodeType type, int startIndex, int length)
		{
			Type = type;
			StartIndex = startIndex;
			Length = length;
		}
	}
}
