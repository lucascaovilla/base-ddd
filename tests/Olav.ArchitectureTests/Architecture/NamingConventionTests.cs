using NetArchTest.Rules;
using Xunit;
using Olav.Testing.Extensions;

namespace Olav.ArchitectureTests.Architecture;

public class NamingConventionTests
{
    [Fact]
    public void Commands_Should_End_With_Command()
    {
        Types.InAssembly(typeof(Olav.Program).Assembly)
            .That()
            .ResideInNamespace("Olav.Commands")
            .Should()
            .HaveNameEndingWith("Command")
            .GetResult()
            .AssertSuccessful("Commands that don't end with 'Command'");
    }

    [Fact]
    public void Generators_Should_End_With_Generator()
    {
        Types.InAssembly(typeof(Olav.Program).Assembly)
            .That()
            .ResideInNamespace("Olav.Generation")
            .Should()
            .HaveNameEndingWith("Generator")
            .GetResult()
            .AssertSuccessful("Generators that don't end with 'Generator'");
    }

    [Fact]
    public void Templates_Should_End_With_Template()
    {
        Types.InAssembly(typeof(Olav.Program).Assembly)
            .That()
            .ResideInNamespace("Olav.Templates")
            .Should()
            .HaveNameEndingWith("Template")
            .GetResult()
            .AssertSuccessful("Templates that don't end with 'Template'");
    }
}
