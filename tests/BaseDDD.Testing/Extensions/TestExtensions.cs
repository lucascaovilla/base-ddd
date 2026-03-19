using System.Linq;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using Xunit;
using NetArchTest.Rules;

namespace BaseDDD.Testing.Extensions;

public static class TestExtensions
{
    public static void AssertSuccessful(
        this TestResult result,
        string? message = null,
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0)
    {
        if (result.IsSuccessful)
            return;

        IEnumerable<string?> failures = result.FailingTypes
            .Select(t => t.FullName ?? t.Name);

        string details = string.Join("\n", failures);

        string location = $"{file}:{line}";

        Assert.Fail(
            $"{message ?? "Architecture rule violated"}\n" +
            $"Location: {location}\n\n" +
            $"{details}");
    }
}