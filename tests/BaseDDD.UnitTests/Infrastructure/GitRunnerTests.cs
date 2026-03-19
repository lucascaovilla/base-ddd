using System;
using System.IO;
using Xunit;

namespace BaseDDD.UnitTests.Infrastructure;

public class GitRunnerTests
{
    [Fact]
    public void Run_Should_Initialize_Git_Repo()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(path);

        BaseDDD.Infrastructure.GitRunner.Run("init", path);

        Assert.True(Directory.Exists(Path.Combine(path, ".git")));
    }
}
