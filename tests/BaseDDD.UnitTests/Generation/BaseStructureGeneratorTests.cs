using System;
using System.IO;
using Xunit;
using BaseDDD.Generation;

namespace BaseDDD.UnitTests.Generation;

public class BaseStructureGeneratorTests
{
    [Fact]
    public void Generate_Should_Create_All_Expected_Directories()
    {
        string root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        BaseStructureGenerator generator = new(root);

        generator.Generate();

        AssertDirectoryExists(root);
        AssertDirectoryExists(Path.Combine(root, "src"));
        AssertDirectoryExists(Path.Combine(root, "tests"));
        AssertDirectoryExists(Path.Combine(root, "docker"));
        AssertDirectoryExists(Path.Combine(root, ".githooks"));
        AssertDirectoryExists(Path.Combine(root, ".github"));
        AssertDirectoryExists(Path.Combine(root, ".github", "workflows"));
    }

    [Fact]
    public void Generate_Should_Not_Throw_When_Directories_Already_Exist()
    {
        string root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        Directory.CreateDirectory(Path.Combine(root, "src"));
        Directory.CreateDirectory(Path.Combine(root, "tests"));

        BaseStructureGenerator generator = new(root);

        Exception exception = Record.Exception(generator.Generate);

        Assert.Null(exception);
    }

    private static void AssertDirectoryExists(string path)
    {
        Assert.True(Directory.Exists(path), $"Expected directory not found: {path}");
    }
}