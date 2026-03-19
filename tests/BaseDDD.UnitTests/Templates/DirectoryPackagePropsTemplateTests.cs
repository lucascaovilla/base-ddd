using Xunit;
using BaseDDD.UnitTests.Templates.Helpers;

namespace BaseDDD.UnitTests.Templates;

public class DirectoryPackagePropsTemplateTests
{
    [Fact]
    public void Should_Be_Valid_MSBuild()
    {
        string content = BaseDDD.Templates.DirectoryPackagePropsTemplate.Generate();

        TemplateValidationHelper.ValidateMsBuild(content);
    }
}
