using NetArchTest.Rules;
using Xunit;
using BaseDDD.Testing.Extensions;

namespace BaseDDD.ArchitectureTests.Architecture;

public class LayerDependencyTests
{
    private const string BaseNamespace = "BaseDDD";

    [Fact]
    public void Commands_Should_Not_Depend_On_Infrastructure()
    {
        Types.InAssembly(typeof(BaseDDD.Program).Assembly)
            .That()
            .ResideInNamespace($"{BaseNamespace}.Commands")
            .ShouldNot()
            .HaveDependencyOn($"{BaseNamespace}.Infrastructure")
            .GetResult()
            .AssertSuccessful("Commands that depend on Infrastructure");
    }

    [Fact]
    public void Generation_Should_Not_Depend_On_Commands()
    {
        Types.InAssembly(typeof(BaseDDD.Program).Assembly)
            .That()
            .ResideInNamespace($"{BaseNamespace}.Generation")
            .ShouldNot()
            .HaveDependencyOn($"{BaseNamespace}.Commands")
            .GetResult()
            .AssertSuccessful("Generators that depend on Commands");
    }

    [Fact]
    public void Infrastructure_Should_Not_Depend_On_Commands_Or_Generation()
    {
        Types.InAssembly(typeof(BaseDDD.Program).Assembly)
            .That()
            .ResideInNamespace($"{BaseNamespace}.Infrastructure")
            .ShouldNot()
            .HaveDependencyOnAny(
                $"{BaseNamespace}.Commands",
                $"{BaseNamespace}.Generation")
            .GetResult()
            .AssertSuccessful("Infrastructure that depends on Commands or Generators");
    }
}
