using Xunit;
using BaseDDD.UnitTests.Templates.Helpers;

namespace BaseDDD.UnitTests.Templates;

public class GlobalJsonTemplateTests
{
    [Fact]
    public void Should_Be_Valid_Json()
    {
        string content = BaseDDD.Templates.GlobalJsonTemplate.Generate();

        TemplateValidationHelper.ValidateJson(content);
    }
}
