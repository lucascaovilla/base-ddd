using Xunit;
using Olav.UnitTests.Templates.Helpers;

namespace Olav.UnitTests.Templates;

public class DirectoryPackagePropsTemplateTests
{
    [Fact]
    public void Should_Be_Valid_MSBuild()
    {
        string content = Olav.Templates.DirectoryPackagePropsTemplate.Generate();

        TemplateValidationHelper.ValidateMsBuild(content);
    }
}
