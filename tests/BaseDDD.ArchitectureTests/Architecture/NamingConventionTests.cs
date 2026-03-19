using NetArchTest.Rules;
using Xunit;
using BaseDDD.Testing.Extensions;

namespace BaseDDD.ArchitectureTests.Architecture;

public class NamingConventionTests
{
    [Fact]
    public void Commands_Should_End_With_Command()
    {
        Types.InAssembly(typeof(BaseDDD.Program).Assembly)
            .That()
            .ResideInNamespace("BaseDDD.Commands")
            .Should()
            .HaveNameEndingWith("Command")
            .GetResult()
            .AssertSuccessful("Commands that don't end with 'Command'");
    }

    [Fact]
    public void Generators_Should_End_With_Generator()
    {
        Types.InAssembly(typeof(BaseDDD.Program).Assembly)
            .That()
            .ResideInNamespace("BaseDDD.Generation")
            .Should()
            .HaveNameEndingWith("Generator")
            .GetResult()
            .AssertSuccessful("Generators that don't end with 'Generator'");
    }

    [Fact]
    public void Templates_Should_End_With_Template()
    {
        Types.InAssembly(typeof(BaseDDD.Program).Assembly)
            .That()
            .ResideInNamespace("BaseDDD.Templates")
            .Should()
            .HaveNameEndingWith("Template")
            .GetResult()
            .AssertSuccessful("Templates that don't end with 'Template'");
    }
}
