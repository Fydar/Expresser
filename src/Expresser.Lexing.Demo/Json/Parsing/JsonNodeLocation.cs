namespace Expresser.Lexing.Demo.Json
{
	public readonly struct JsonNodeLocation
	{
		public enum LocationType
		{
			Array,
			Object,
			Value
		}

		public LocationType Type { get; }

		public JsonNodeLocation(LocationType type)
		{
			Type = type;
		}
	}
}
