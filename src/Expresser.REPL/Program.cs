using Expresser.Syntax;
using System;
using System.Diagnostics;
using System.IO;

namespace Expresser.REPL
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			while (true)
			{
				Console.ForegroundColor = ConsoleColor.Gray;
				Console.Write("> ");
				string inputExpression = Console.ReadLine();

				MathValue result;
				try
				{
					var expression = CompiledExpression.Compile(inputExpression);

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
	}
}
