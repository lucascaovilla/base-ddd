using Xunit;
using BaseDDD.UnitTests.Templates.Helpers;

namespace BaseDDD.UnitTests.Templates;

public class DirectoryBuildPropsTemplateTests
{
    [Fact]
    public void Should_Be_Valid_MSBuild()
    {
        string content = BaseDDD.Templates.DirectoryBuildPropsTemplate.Generate();

        TemplateValidationHelper.ValidateMsBuild(content);
    }
}
