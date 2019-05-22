# Expresser

[![NuGet](https://img.shields.io/nuget/v/Expresser.svg)](https://www.nuget.org/packages/expresser) [![NuGet](https://img.shields.io/nuget/dt/expresser.svg)](https://www.nuget.org/packages/expresser)

Simple mathematics library for C#, optimized for performance in data-driven games.

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

        var expression = new CompiledExpression("0.1*Width", context);

        var result = expression.Evaluate();

        Console.WriteLine (expression);        // 0.1 * Width
        Console.WriteLine (result.ValueClass); // ValueClassifier.Float
        Console.WriteLine (result.FloatValue); // 1
    }
}
```

## Todo

![Project Backlog](https://img.shields.io/github/issues/Fydar/Expresser/Project_Backlog.svg?color=%2384c649&label=Project%20Backlog)

Issues marked with Todo are issues that have been identified as potential enhancements for Expresser, and are apart of the project backlog.

Below is a collection of enhancements that are currently being considered for the project backlog.

- Introduce another intermediary form of an expression to increase evaluation performance.
- Support for `%` operator in use in defining percentages
- Support for `!` as a NOT operator
- Support for bitwise operators
- Introduce "Functions" (`max(5, 10)`, `mod(11.5)`)
- More Data-Types (currently only support Bool and Float)

## Known Issues

![Bugs](https://img.shields.io/github/issues/Fydar/Expresser/bug.svg?color=%23c66649&label=Bugs)

Expresser is still in the early stages of development with occasional breaking changes.

## Contributing

Feel free to submit pull requests, referencing any relevant issues and listing a brief overview of the changes in the pull request.

### Commit Messages

[![Gitmoji](https://img.shields.io/badge/gitmoji-%20😜%20😍-FFDD67.svg?style=flat-square)](https://gitmoji.carloscuesta.me)

This project follows commit guidelines specified by [Gitmoji](https://gitmoji.carloscuesta.me).

Commit messages should have around a 50 character long headline, with any addition information following two line breaks. [Fork](https://git-fork.com/) uses this format when you are writing your commit messages.

### Code Style

An `.editorconfig` is included in the project that will ensure that your IDE follows the projects code style. I recommend you use an IDE that supports `.editorconfig` files such as [Visual Studio](https://visualstudio.microsoft.com/).

## License

This work is licensed under the **Apache License, Version 2.0**, meaning you are free to use this work commercially under the conditions the `LICENSE` and `NOTICE` files are included within the source.
