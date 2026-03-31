using System;
using Xunit;

namespace Olav.UnitTests.Infrastructure;

public class ProcessRunnerTests
{
    [Fact]
    public void Run_Should_Execute_Command()
    {
        Exception ex = Record.Exception(() =>
            Olav.Infrastructure.ProcessRunner.Run("dotnet", "--version", ".")
        );

        Assert.Null(ex);
    }
}
