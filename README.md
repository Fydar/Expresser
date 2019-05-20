# Expresser

[![NuGet](https://img.shields.io/nuget/v/Expresser.svg)](https://www.nuget.org/packages/expresser)

Simple mathematics library for C#, optimized for performance in data-driven games.

## Project State

This project came about as a result of a personal project of mine requiring a way of expression conditions in data. As a result, there are still many things that this project lacks. If your looking for a more complete implementation I highly recommend [ExpressionEvaluator](https://github.com/codingseb/ExpressionEvaluator).

## Usage

```csharp
using System;
using Expresser;

public class Program
{
    public static void Main(string[] args)
    {
        var context = new MathContextBuilder()
            .WithTerm("Width", new StaticValueProvider(10))
            .Build();

        var expression = new CompiledExpression("0.1*Width");

        var result = expression.Evaluate();

        Console.WriteLine (expression);        // 0.1 * source[0]
        Console.WriteLine (result.ValueClass); // ValueClassifier.Float
        Console.WriteLine (result.FloatValue); // 1
    }
}
```

## Todo

- Introduce another intermediary form of an expression to increase evaluation performance.
- Support for `%` operator in use in defining percentages
- Support for `!` as a NOT operator
- Support for bitwise operators
- Introduce "Functions" (`max(5, 10)`, `mod(11.5)`)
- More Data-Types (currently only support Bool and Float)
