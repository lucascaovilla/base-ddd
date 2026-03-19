using Xunit;
using BaseDDD.UnitTests.Templates.Helpers;

namespace BaseDDD.UnitTests.Templates;

public class PreCommitTemplateTests
{
    [Fact]
    public void Should_Be_Valid_Shell()
    {
        string content = BaseDDD.Templates.PreCommitTemplate.Generate();

        TemplateValidationHelper.ValidateShell(content);
    }
}
