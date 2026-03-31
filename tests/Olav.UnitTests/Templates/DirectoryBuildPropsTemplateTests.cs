using Xunit;
using Olav.UnitTests.Templates.Helpers;

namespace Olav.UnitTests.Templates;

public class DirectoryBuildPropsTemplateTests
{
    [Fact]
    public void Should_Be_Valid_MSBuild()
    {
        string content = Olav.Templates.DirectoryBuildPropsTemplate.Generate();

        TemplateValidationHelper.ValidateMsBuild(content);
    }
}
