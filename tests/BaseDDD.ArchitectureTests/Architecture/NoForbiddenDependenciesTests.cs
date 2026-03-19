using NetArchTest.Rules;
using Xunit;
using System.Linq;
using BaseDDD.Testing.Extensions;

namespace BaseDDD.ArchitectureTests.Architecture;

public class NoForbiddenDependenciesTests
{
    private const string BaseNamespace = "BaseDDD";

    [Fact]
    public void Only_Generation_Should_Use_System_IO()
    {
        Types.InAssembly(typeof(Program).Assembly)
            .That()
            .DoNotResideInNamespace("BaseDDD.Generation")
            .And()
            .DoNotResideInNamespace("BaseDDD.Infrastructure")
            .And()
            .DoNotResideInNamespace("BaseDDD.Commands")
            .And()
            .DoNotResideInNamespace("BaseDDD.Verifiers")
            .And()
            .DoNotResideInNamespace("BaseDDD.Helpers")
            .ShouldNot()
            .HaveDependencyOn("System.IO")
            .GetResult()
            .AssertSuccessful("Types using System.IO");
    }

    [Fact]
    public void Only_Infrastructure_Should_Use_System_Diagnostics()
    {
        Types.InAssembly(typeof(BaseDDD.Program).Assembly)
            .That()
            .DoNotResideInNamespace($"{BaseNamespace}.Infrastructure")
            .ShouldNot()
            .HaveDependencyOn("System.Diagnostics")
            .GetResult()
            .AssertSuccessful("Infrastructures that don't use System.Diagnostics");
    }
}
