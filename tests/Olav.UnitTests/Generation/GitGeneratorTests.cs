using System;
using System.IO;
using Xunit;
using Olav.Generation;

namespace Olav.UnitTests.Generation;

public class GitGeneratorTests
{
    [Fact]
    public void Generate_Should_Initialize_Git_Repository_And_Configure_Hooks()
    {
        string root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(root);

        File.WriteAllText(Path.Combine(root, "README.md"), "test");

        Directory.CreateDirectory(Path.Combine(root, ".githooks"));
        File.WriteAllText(Path.Combine(root, ".githooks/pre-commit"), "echo test");
        File.WriteAllText(Path.Combine(root, ".githooks/pre-push"), "echo test");

        new GitGenerator(root).Generate();

        string gitDir = Path.Combine(root, ".git");
        Assert.True(Directory.Exists(gitDir), ".git folder not created");
        Assert.True(File.Exists(Path.Combine(gitDir, "HEAD")), "HEAD not found");
        Assert.True(File.Exists(Path.Combine(gitDir, "config")), "config not found");

        string config = File.ReadAllText(Path.Combine(gitDir, "config"));
        Assert.Contains("hooksPath", config);
        Assert.Contains(".githooks", config);
    }

    [Fact]
    public void Generate_Should_Be_Idempotent()
    {
        string root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(root);

        File.WriteAllText(Path.Combine(root, "README.md"), "test");

        Directory.CreateDirectory(Path.Combine(root, ".githooks"));
        File.WriteAllText(Path.Combine(root, ".githooks/pre-commit"), "echo test");
        File.WriteAllText(Path.Combine(root, ".githooks/pre-push"), "echo test");

        GitGenerator generator = new(root);
        generator.Generate();

        Exception exception = Record.Exception(generator.Generate);

        Assert.Null(exception);
    }
}
