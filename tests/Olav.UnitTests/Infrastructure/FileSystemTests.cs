using System;
using System.IO;
using Xunit;

namespace Olav.UnitTests.Infrastructure;

public class FileSystemTests
{
    [Fact]
    public void WriteFile_Should_Create_File_And_Content()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString(), "test.txt");

        Olav.Infrastructure.FileSystem.WriteFile(path, "content");

        Assert.True(File.Exists(path));
        Assert.Equal("content", File.ReadAllText(path));
    }

    [Fact]
    public void CreateDirectory_Should_Create_Directory()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        Olav.Infrastructure.FileSystem.CreateDirectory(path);

        Assert.True(Directory.Exists(path));
    }

    [Fact]
    public void DeleteIfExists_Should_Delete_File()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        File.WriteAllText(path, "test");

        Olav.Infrastructure.FileSystem.DeleteIfExists(path);

        Assert.False(File.Exists(path));
    }
}
