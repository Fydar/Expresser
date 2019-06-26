using BenchmarkDotNet.Running;
using System;

namespace Expresser.Benchmarks
{

	public class Program
	{
		public static void Main (string[] args)
		{
			var summary = BenchmarkRunner.Run<BasicBenchmarks> ();

			Console.WriteLine (summary);
		}
	}
}
