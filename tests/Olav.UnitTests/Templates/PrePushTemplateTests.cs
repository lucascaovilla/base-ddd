using Xunit;
using Olav.UnitTests.Templates.Helpers;

namespace Olav.UnitTests.Templates;

public class PrePushTemplateTests
{
    [Fact]
    public void Should_Be_Valid_Shell()
    {
        string content = Olav.Templates.PrePushTemplate.Generate();

        TemplateValidationHelper.ValidateShell(content);
    }
}
