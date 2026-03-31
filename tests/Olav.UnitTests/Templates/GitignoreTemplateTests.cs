using Xunit;
using Olav.UnitTests.Templates.Helpers;

namespace Olav.UnitTests.Templates;

public class GitignoreTemplateTests
{
    [Fact]
    public void Should_Not_Be_Empty()
    {
        string content = Olav.Templates.GitignoreTemplate.Generate();

        Assert.False(string.IsNullOrWhiteSpace(content));
    }
}
