using Expresser.Language.CSharp;
using Expresser.Language.Json;
using Expresser.Language.Json.Parsing;
using Expresser.Lexing;
using System;
using System.IO;

namespace Expresser.Demo
{
	internal class Program
	{
		public static void Main()
		{
			var csharpLexer = new Lexer(new CSharpLexerLanguage());
			string csharpExample = ReadExampleResource("Expresser.Lexing.Demo.CSharp.example1.txt");
			Highlight(csharpLexer, csharpExample);
			Describe(csharpLexer, csharpExample);

			var jsonLexer = new Lexer(new JsonLexerLanguage());
			string jsonExample = ReadExampleResource("Expresser.Lexing.Demo.Json.example1.json");
			Highlight(jsonLexer, jsonExample);
			Describe(jsonLexer, jsonExample);

			var jsonParser = new JsonParser();
			Highlight(jsonParser, jsonExample);
		}

		private static void Highlight(JsonParser parser, string source)
		{
			foreach (var token in parser.Parse(source))
			{
				string tokenContent = source.Substring(token.StartIndex, token.Length);

				ConsoleColor color;
				switch (token.Type)
				{
					default:
					case JsonNodeType.Newline:
					case JsonNodeType.Whitespace:
					case JsonNodeType.PropertyValueDeliminator:
					case JsonNodeType.ValueDeliminator:
					case JsonNodeType.EndArray:
					case JsonNodeType.StartArray:
					case JsonNodeType.EndObject:
					case JsonNodeType.StartObject:
					{
						color = ConsoleColor.DarkGray;
						break;
					}
					case JsonNodeType.PropertyName:
					{
						color = ConsoleColor.Yellow;
						break;
					}
					case JsonNodeType.MultiLineComment:
					case JsonNodeType.SingleLineComment:
					{
						color = ConsoleColor.Green;
						break;
					}
					case JsonNodeType.StringLiteral:
					{
						color = ConsoleColor.DarkYellow;
						break;
					}
					case JsonNodeType.NumberLiteral:
					{
						color = ConsoleColor.DarkCyan;
						break;
					}
					case JsonNodeType.TrueLiteral:
					case JsonNodeType.NullLiteral:
					case JsonNodeType.FalseLiteral:
					{
						color = ConsoleColor.Cyan;
						break;
					}
				}

				Console.ForegroundColor = color;
				Console.Write(tokenContent);
			}
			Console.Write("\n");
		}

		private static void Highlight(Lexer lexer, string source)
		{
			foreach (var token in lexer.Tokenize(source))
			{
				string tokenContent = source.Substring(token.StartIndex, token.Length);

				Console.ForegroundColor = token.Classifier == -1
					? ConsoleColor.Red
					: lexer.LexerLanguage.Colors[token.Classifier];

				Console.Write(tokenContent);
			}
			Console.Write("\n");
		}

		private static void Describe(Lexer lexer, string source)
		{
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine(new string('=', 52));

			Console.ForegroundColor = ConsoleColor.Gray;

			int index = 0;
			foreach (var token in lexer.Tokenize(source))
			{
				string tokenContent = source.Substring(token.StartIndex, token.Length);
				string rendered = tokenContent;

				if (string.IsNullOrWhiteSpace(rendered))
				{
					rendered = rendered.Replace(Environment.NewLine, "\\n")
						.Replace(' ', '.');
				}

				string typeName;
				if (token.Classifier == -1)
				{
					typeName = "Invalid".PadRight(16);
				}
				else
				{
					var classifier = lexer.LexerLanguage.Classifiers[token.Classifier];
					typeName = classifier.GetType().Name
						.Replace("TokenClassifier", "")
						.PadRight(16);
				}

				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write($"ID:{index,4}  Type:{token.Classifier,4} ");
				if (token.Classifier == -1)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write(typeName);
				}
				else
				{
					Console.ForegroundColor = ConsoleColor.Gray;
					Console.Write(typeName);
				}
				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write("\tSymbol: ");

				if (token.Classifier == -1)
				{
					Console.ForegroundColor = ConsoleColor.Red;
				}
				else
				{
					Console.ForegroundColor = lexer.LexerLanguage.Colors[token.Classifier];
				}
				Console.Write(rendered);
				Console.Write("\n");

				index++;
			}

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine(new string('=', 52));
			Console.Write("\n");
		}

		private static string ReadExampleResource(string name)
		{
			var assembly = typeof(Program).Assembly;

			using var stream = assembly.GetManifestResourceStream(name);

			if (stream == null)
			{
				throw new InvalidOperationException($"Cannot locate assembly-embedded resource \"{name}\".");
			}

			using var streamReader = new StreamReader(stream);
			return streamReader.ReadToEnd();
		}
	}
}
