using System;
using Xunit;

namespace BaseDDD.UnitTests.Infrastructure;

public class DotnetRunnerTests
{
    [Fact]
    public void Run_Should_Execute_Dotnet_Command()
    {
        Exception ex = Record.Exception(() =>
            BaseDDD.Infrastructure.DotnetRunner.Run("--version", ".")
        );

        Assert.Null(ex);
    }
}
