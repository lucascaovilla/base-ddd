using Xunit;
using BaseDDD.UnitTests.Templates.Helpers;

namespace BaseDDD.UnitTests.Templates;

public class EditorConfigTemplateTests
{
    [Fact]
    public void Should_Not_Be_Empty()
    {
        string content = BaseDDD.Templates.EditorConfigTemplate.Generate();

        Assert.False(string.IsNullOrWhiteSpace(content));
        Assert.Contains("root", content);
    }
}
