using System;
using System.IO;

namespace BaseDDD.IntegrationTests.Generation.Fixtures;

public class GeneratedProjectFixture : IDisposable
{
    public string Root { get; }
    public string ProjectPath { get; }
    public string ProjectName { get; } = "TestProject";
    private readonly string originalDirectory;

    public GeneratedProjectFixture()
    {
        this.originalDirectory = Directory.GetCurrentDirectory();

        Root = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(Root);

        Directory.SetCurrentDirectory(Root);

        BaseDDD.Program.Main(["new", ProjectName]);

        ProjectPath = Path.Combine(Root, ProjectName);
    }

    public void Dispose()
    {
        Directory.SetCurrentDirectory(this.originalDirectory);
        Directory.Delete(Root, true);
    }
}
