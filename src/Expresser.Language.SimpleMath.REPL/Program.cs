using Expresser.Language.SimpleMath.Lexing;
using Expresser.Language.SimpleMath.Runtime;
using Expresser.Lexing;
using System;
using System.Diagnostics;
using System.IO;

namespace Expresser.REPL
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var lexer = new Lexer(new SimpleMathLexerLanguage());

			while (true)
			{
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write("> ");
				string inputExpression = Console.ReadLine();

				Highlight(lexer, inputExpression);
				Describe(lexer, inputExpression);

				MathValue result;
				try
				{
					var expression = SimpleMathExpression.Compile(inputExpression);

					result = expression.Evaluate();
				}
				catch (Exception exception)
				{
					WriteFormattedException(exception);
					continue;
				}

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine($"Result: {result}");
			}
		}

		private static void WriteFormattedException(Exception exception)
		{
			string demystifiedException = exception.ToStringDemystified();

			var originalColor = Console.ForegroundColor;

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write("╔");
			Console.Write(new string('═', Console.WindowWidth - 2));
			Console.Write("\n");

			using (var reader = new StringReader(demystifiedException))
			{
				string line;
				while ((line = reader.ReadLine()) != null)
				{
					var lineSpan = line.AsSpan();
					int inIndex = lineSpan.IndexOf(") in ");

					Console.ForegroundColor = ConsoleColor.DarkGray;
					Console.Write("║");

					if (inIndex != -1)
					{
						Console.ForegroundColor = ConsoleColor.Gray;
						Console.Write(lineSpan.Slice(0, inIndex + 1).ToString());

						Console.ForegroundColor = ConsoleColor.DarkGray;
						Console.Write("\n║    ");
						Console.Write(lineSpan.Slice(inIndex + 1).ToString());
						Console.Write("\n");
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.Gray;
						Console.Write(lineSpan.ToString());
						Console.Write("\n");
					}
				}
			}

			Console.ForegroundColor = ConsoleColor.DarkGray;
			Console.Write("╚");
			Console.Write(new string('═', Console.WindowWidth - 2));
			Console.Write("\n");

			Console.ForegroundColor = originalColor;
		}

		private static void Highlight(Lexer lexer, string source)
		{
			foreach (var token in lexer.Tokenize(source))
			{
				string tokenContent = source.Substring(token.StartIndex, token.Length);
				if (token.Classifier == -1)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write(tokenContent);
				}
				else
				{
					Console.ForegroundColor = lexer.LexerLanguage.Colors[token.Classifier];
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
			foreach (var token in lexer.Tokenize(source))
			{
				string tokenContent = source.Substring(token.StartIndex, token.Length);
				string rendered = tokenContent.Replace(Environment.NewLine, "\\n");
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
	}
}
