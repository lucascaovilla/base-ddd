using Olav.Testing.Extensions;
using NetArchTest.Rules;
using Xunit;

namespace Olav.ArchitectureTests.Architecture;

public class TemplatesIsolationTests
{
    private const string BaseNamespace = "Olav";

    [Fact]
    public void Templates_Should_Not_Depend_On_Commands()
    {
        Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace($"{BaseNamespace}.Templates")
            .ShouldNot()
            .HaveDependencyOn($"{BaseNamespace}.Commands")
            .GetResult()
            .AssertSuccessful("Templates that depend on Commands");
    }

    [Fact]
    public void Templates_Should_Not_Depend_On_Infrastructure()
    {
        Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace($"{BaseNamespace}.Templates")
            .ShouldNot()
            .HaveDependencyOn($"{BaseNamespace}.Infrastructure")
            .GetResult()
            .AssertSuccessful("Templates that depend on Infrastructure");
    }

    [Fact]
    public void Templates_Should_Not_Depend_On_Generation()
    {
        Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace($"{BaseNamespace}.Templates")
            .ShouldNot()
            .HaveDependencyOn($"{BaseNamespace}.Generation")
            .GetResult()
            .AssertSuccessful("Templates that depend on Generation");
    }

    [Fact]
    public void Templates_Should_Not_Depend_On_Helpers()
    {
        Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace($"{BaseNamespace}.Templates")
            .ShouldNot()
            .HaveDependencyOn($"{BaseNamespace}.Helpers")
            .GetResult()
            .AssertSuccessful("Templates that depend on Helpers");
    }

    [Fact]
    public void Templates_Should_Not_Depend_On_Verifiers()
    {
        Types.InAssembly(typeof(Program).Assembly)
            .That()
            .ResideInNamespace($"{BaseNamespace}.Templates")
            .ShouldNot()
            .HaveDependencyOn($"{BaseNamespace}.Verifiers")
            .GetResult()
            .AssertSuccessful("Templates that depend on Verifiers");
    }
}
