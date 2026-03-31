using Xunit;
using Olav.UnitTests.Templates.Helpers;

namespace Olav.UnitTests.Templates;

public class GlobalJsonTemplateTests
{
    [Fact]
    public void Should_Be_Valid_Json()
    {
        string content = Olav.Templates.GlobalJsonTemplate.Generate();

        TemplateValidationHelper.ValidateJson(content);
    }
}
