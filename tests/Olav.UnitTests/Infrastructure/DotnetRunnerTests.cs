using System;
using Xunit;

namespace Olav.UnitTests.Infrastructure;

public class DotnetRunnerTests
{
    [Fact]
    public void Run_Should_Execute_Dotnet_Command()
    {
        Exception ex = Record.Exception(() =>
            Olav.Infrastructure.DotnetRunner.Run("--version", ".")
        );

        Assert.Null(ex);
    }
}
