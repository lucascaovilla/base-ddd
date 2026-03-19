using Xunit;
using BaseDDD.UnitTests.Templates.Helpers;

namespace BaseDDD.UnitTests.Templates;

public class GitignoreTemplateTests
{
    [Fact]
    public void Should_Not_Be_Empty()
    {
        string content = BaseDDD.Templates.GitignoreTemplate.Generate();

        Assert.False(string.IsNullOrWhiteSpace(content));
    }
}
