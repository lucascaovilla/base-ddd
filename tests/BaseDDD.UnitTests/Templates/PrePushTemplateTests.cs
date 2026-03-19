using Xunit;
using BaseDDD.UnitTests.Templates.Helpers;

namespace BaseDDD.UnitTests.Templates;

public class PrePushTemplateTests
{
    [Fact]
    public void Should_Be_Valid_Shell()
    {
        string content = BaseDDD.Templates.PrePushTemplate.Generate();

        TemplateValidationHelper.ValidateShell(content);
    }
}
