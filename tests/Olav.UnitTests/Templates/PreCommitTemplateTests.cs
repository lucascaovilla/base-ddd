using Xunit;
using Olav.UnitTests.Templates.Helpers;

namespace Olav.UnitTests.Templates;

public class PreCommitTemplateTests
{
    [Fact]
    public void Should_Be_Valid_Shell()
    {
        string content = Olav.Templates.PreCommitTemplate.Generate();

        TemplateValidationHelper.ValidateShell(content);
    }
}
