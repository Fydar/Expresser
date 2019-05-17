using System;

namespace ExpressionMathmatics
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Hello World!");

			Console.WriteLine(new CompiledExpression("3 ^ (1 + 2)").Evaluate());
			Console.WriteLine(new CompiledExpression("((1 + 3) + 1)").Evaluate());

			Console.WriteLine(new CompiledExpression("10+15").Evaluate());
			Console.WriteLine(new CompiledExpression("10 + 10").Evaluate());
			Console.WriteLine(new CompiledExpression("10 + 10 + 5").Evaluate());

			Console.WriteLine(new CompiledExpression("10 - 10 + 5").Evaluate());

			Console.WriteLine(new CompiledExpression("200 * 0.1").Evaluate());

			Console.WriteLine(new CompiledExpression("3 ^ 3").Evaluate());

			Console.WriteLine(new CompiledExpression("10px + 15%").Evaluate());

			Console.WriteLine(new CompiledExpression("10px + -Height").Evaluate());

		}
	}
}
