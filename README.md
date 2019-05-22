# Expresser

[![NuGet](https://img.shields.io/nuget/v/Expresser.svg)](https://www.nuget.org/packages/expresser) [![NuGet](https://img.shields.io/nuget/dt/expresser.svg)](https://www.nuget.org/packages/expresser)

Simple mathematics library for C#, optimized for performance in data-driven games.

[![Documentation](https://img.shields.io/badge/-Documentation-278bce.svg)](https://github.com/Fydar/Expresser/wiki/Expresser)

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

[![Project Backlog](https://img.shields.io/github/issues/Fydar/Expresser/todo.svg?color=%2384c649&label=Todo%20List)](https://github.com/Fydar/Expresser/issues?q=is%3Aissue+is%3Aopen+label%3Atodo)
[![Bugs](https://img.shields.io/github/issues/Fydar/Expresser/bug.svg?color=%23c66649&label=Bugs)](https://github.com/Fydar/Expresser/issues?q=is%3Aissue+is%3Aopen+label%3Abug)

Issues marked with Todo are issues that have been identified as potential enhancements for Expresser, and are apart of the project backlog.

Below is a collection of enhancements that are currently being considered for the project backlog.

- Introduce "Functions" (`max(5, 10)`, `mod(11.5)`) [#1](https://github.com/Fydar/Expresser/issues/1)
- Introduce another intermediary form of an expression to increase evaluation performance.
- Support for `%` operator in use in defining percentages
- Support for `!` as a NOT operator
- Support for bitwise operators
- More Data-Types (currently only support Bool and Float)

## Contributing

[![Gitmoji](https://img.shields.io/badge/gitmoji-%20😜%20😍-FFDD67.svg)](https://gitmoji.carloscuesta.me)

This project follows commit guidelines specified by [Gitmoji](https://gitmoji.carloscuesta.me), with the exception of using the raw unicode character (✨) rather than a string (`:sparkles:`). This is to increase compatibility with Git clients.

Commit messages should target around a 50 character long headline, with any addition information following two line breaks. [Fork](https://git-fork.com/) uses this format when you are writing your commit messages.

Feel free to submit pull requests, referencing any relevant issues and listing a brief overview of the changes in the pull request.

### Code Style

An `.editorconfig` is included in the project that will ensure that your IDE follows the projects code style. I recommend you use an IDE that supports `.editorconfig` files such as [Visual Studio](https://visualstudio.microsoft.com/).

## License

[![License](https://img.shields.io/github/license/Fydar/Expresser.svg)](https://github.com/Fydar/Expresser/blob/master/LICENSE)

This work is licensed under the **Apache License, Version 2.0**, meaning you are free to use this work commercially under the conditions the `LICENSE` and `NOTICE` files are included within the source.
