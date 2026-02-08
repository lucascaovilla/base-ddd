if (args.Length == 0)
{
    PrintHelp();
    return;
}

var command = args[0].ToLowerInvariant();

switch (command)
{
    case "new":
        HandleNew(args);
        break;

    case "lint":
        HandleLint();
        break;

    case "verify":
        HandleVerify();
        break;

    default:
        Console.WriteLine($"Unknown command: {command}");
        PrintHelp();
        break;
}

static void HandleNew(string[] args)
{
    if (args.Length < 2)
    {
        Console.WriteLine("Project name is required.");
        return;
    }

    var name = args[1];
    var rootPath = Path.Combine(Directory.GetCurrentDirectory(), name);

    if (Directory.Exists(rootPath))
    {
        Console.WriteLine("Directory already exists.");
        return;
    }

    Console.WriteLine($"Creating BaseDDD project: {name}");

    // Create base folders
    Directory.CreateDirectory(rootPath);
    Directory.CreateDirectory(Path.Combine(rootPath, "src"));
    Directory.CreateDirectory(Path.Combine(rootPath, "tests"));
    Directory.CreateDirectory(Path.Combine(rootPath, "docker"));

    // Create solution
    RunDotnet($"new sln -n {name}", rootPath);

    var srcPath = Path.Combine(rootPath, "src");
    var testsPath = Path.Combine(rootPath, "tests");

    // Create Projects (src)
    RunDotnet($"new classlib -n {name}.Domain -f net10.0", srcPath);
    RunDotnet($"new classlib -n {name}.Application -f net10.0", srcPath);
    RunDotnet($"new classlib -n {name}.Infrastructure -f net10.0", srcPath);
    RunDotnet($"new webapi -n {name}.Web -f net10.0 --no-https", srcPath);

    // Create Projects (tests)
    RunDotnet($"new xunit -n {name}.ArchitectureTests -f net10.0", testsPath);
    RunDotnet($"new xunit -n {name}.IntegrationTests -f net10.0", testsPath);

    // Add projects to solution
    RunDotnet($"sln add src/{name}.Domain/{name}.Domain.csproj", rootPath);
    RunDotnet($"sln add src/{name}.Application/{name}.Application.csproj", rootPath);
    RunDotnet($"sln add src/{name}.Infrastructure/{name}.Infrastructure.csproj", rootPath);
    RunDotnet($"sln add src/{name}.Web/{name}.Web.csproj", rootPath);

    RunDotnet($"sln add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj", rootPath);
    RunDotnet($"sln add tests/{name}.IntegrationTests/{name}.IntegrationTests.csproj", rootPath);

    // Add project references (enforce clean architecture direction)

    // Application -> Domain
    RunDotnet($"add src/{name}.Application/{name}.Application.csproj reference src/{name}.Domain/{name}.Domain.csproj", rootPath);

    // Infrastructure -> Application + Domain
    RunDotnet($"add src/{name}.Infrastructure/{name}.Infrastructure.csproj reference src/{name}.Application/{name}.Application.csproj", rootPath);
    RunDotnet($"add src/{name}.Infrastructure/{name}.Infrastructure.csproj reference src/{name}.Domain/{name}.Domain.csproj", rootPath);

    // Web -> Application
    RunDotnet($"add src/{name}.Web/{name}.Web.csproj reference src/{name}.Application/{name}.Application.csproj", rootPath);

    // ArchitectureTests -> Domain + Application + Infrastructure + Web
    RunDotnet($"add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj reference src/{name}.Domain/{name}.Domain.csproj", rootPath);
    RunDotnet($"add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj reference src/{name}.Application/{name}.Application.csproj", rootPath);
    RunDotnet($"add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj reference src/{name}.Infrastructure/{name}.Infrastructure.csproj", rootPath);
    RunDotnet($"add tests/{name}.ArchitectureTests/{name}.ArchitectureTests.csproj reference src/{name}.Web/{name}.Web.csproj", rootPath);

    // IntegrationTests -> Web
    RunDotnet($"add tests/{name}.IntegrationTests/{name}.IntegrationTests.csproj reference src/{name}.Web/{name}.Web.csproj", rootPath);

    // Cleanup default template junk

    DeleteIfExists(Path.Combine(srcPath, $"{name}.Domain/Class1.cs"));
    DeleteIfExists(Path.Combine(srcPath, $"{name}.Application/Class1.cs"));
    DeleteIfExists(Path.Combine(srcPath, $"{name}.Infrastructure/Class1.cs"));

    DeleteIfExists(Path.Combine(testsPath, $"{name}.ArchitectureTests/UnitTest1.cs"));
    DeleteIfExists(Path.Combine(testsPath, $"{name}.IntegrationTests/UnitTest1.cs"));

    DeleteIfExists(Path.Combine(srcPath, $"{name}.Web/Controllers/WeatherForecastController.cs"));
    DeleteIfExists(Path.Combine(srcPath, $"{name}.Web/WeatherForecast.cs"));

    Console.WriteLine("BaseDDD solution created successfully.");
}

static void HandleLint()
{
    Console.WriteLine("Running structure lint...");
}

static void HandleVerify()
{
    Console.WriteLine("Verifying architecture...");
}

static void PrintHelp()
{
    Console.WriteLine("""
BaseDDD CLI

Usage:
  baseddd new <ProjectName>
  baseddd lint
  baseddd verify
""");
}

static void RunDotnet(string arguments, string workingDirectory)
{
    var process = new System.Diagnostics.Process
    {
        StartInfo = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = arguments,
            WorkingDirectory = workingDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false
        }
    };

    process.Start();
    process.WaitForExit();

    if (process.ExitCode != 0)
    {
        throw new Exception($"dotnet {arguments} failed.");
    }
}

static void DeleteIfExists(string path)
{
    if (File.Exists(path))
        File.Delete(path);
}
