using Xunit;
using Olav.UnitTests.Templates.Helpers;

namespace Olav.UnitTests.Templates;

public class EditorConfigTemplateTests
{
    [Fact]
    public void Should_Not_Be_Empty()
    {
        string content = Olav.Templates.EditorConfigTemplate.Generate();

        Assert.False(string.IsNullOrWhiteSpace(content));
        Assert.Contains("root", content);
    }
}
