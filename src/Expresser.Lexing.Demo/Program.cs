using Expresser.Demo.CSharp;
using Expresser.Demo.Json;
using Expresser.Lexing;
using System;
using System.IO;

namespace Expresser.Demo
{
	internal class Program
	{
		public static void Main()
		{
			var csharpLexer = new Lexer(new CSharpLang());
			string csharpExample = ReadExampleResource("Expresser.Lexing.Demo.CSharp.example1.txt");
			Highlight(csharpLexer, csharpExample);
			Describe(csharpLexer, csharpExample);

			var jsonLexer = new Lexer(new JsonLang());
			string jsonExample = ReadExampleResource("Expresser.Lexing.Demo.Json.example1.json");
			Highlight(jsonLexer, jsonExample);
			Describe(jsonLexer, jsonExample);
		}

		private static void Highlight(Lexer lexer, string source)
		{
			foreach (var token in lexer.Evaluate(source))
			{
				string tokenContent = source.Substring(token.StartIndex, token.Length);
				if (token.ClassifierIndex == -1)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write(tokenContent);
				}
				else
				{
					Console.ForegroundColor = lexer.LexerLanguage.Colors[token.ClassifierIndex];
					Console.Write(tokenContent);
				}
			}
			Console.Write("\n");
		}

		private static void Describe(Lexer lexer, string source)
		{
			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.WriteLine(new string('=', 52));

			Console.ForegroundColor = ConsoleColor.Gray;

			int index = 0;
			foreach (var token in lexer.Evaluate(source))
			{
				string tokenContent = source.Substring(token.StartIndex, token.Length);
				string rendered = tokenContent.Replace(Environment.NewLine, "\\n");
				string typeName;
				if (token.ClassifierIndex == -1)
				{
					typeName = "Invalid".PadRight(16);
				}
				else
				{
					var classifier = lexer.LexerLanguage.Classifiers[token.ClassifierIndex];
					typeName = classifier.GetType().Name
						.Replace("TokenClassifier", "")
						.PadRight(16);
				}


				Console.ForegroundColor = ConsoleColor.DarkGray;
				Console.Write($"ID:{index,4}  Type:{token.ClassifierIndex,4} ");
				if (token.ClassifierIndex == -1)
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

				if (token.ClassifierIndex == -1)
				{
					Console.ForegroundColor = ConsoleColor.Red;
				}
				else
				{
					Console.ForegroundColor = lexer.LexerLanguage.Colors[token.ClassifierIndex];
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
			using var streamReader = new StreamReader(stream);
			return streamReader.ReadToEnd();
		}
	}
}
