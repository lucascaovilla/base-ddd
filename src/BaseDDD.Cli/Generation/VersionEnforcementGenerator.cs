// <copyright file="VersionEnforcementGenerator.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Generation;

using System.Text.Json;
using BaseDDD.Infrastructure;
using BaseDDD.Templates;

/// <summary>
/// Generates and migrates the baseddd.json version enforcement file.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="VersionEnforcementGenerator"/> class.
/// </remarks>
/// <param name="root">Repository's root.</param>
public class VersionEnforcementGenerator(string root)
{
    private readonly string root = root;
    private readonly string toolVersion = VersionConstants.ToolVersion;
    private readonly string templateVersion = VersionConstants.TemplateVersion;

    /// <summary>
    /// Generates a fresh baseddd.json at the project root.
    /// Fails loudly if one already exists.
    /// </summary>
    public void Generate()
    {
        string path = Path.Combine(this.root, "baseddd.json");

        if (File.Exists(path))
        {
            throw new InvalidOperationException(
                $"baseddd.json already exists at '{path}'. " +
                $"This directory is already a BaseDDD project. " +
                $"To upgrade it, run 'baseddd migrate' instead.");
        }

        DateTime now = DateTime.UtcNow;

        FileSystem.WriteFile(
            path,
            BaseDddJsonTemplate.Generate(this.toolVersion, this.templateVersion, now, now));

        Console.WriteLine($"Project template created on version {this.templateVersion} with tool version {this.toolVersion}.");
    }

    /// <summary>
    /// Migrates baseddd.json to the current template version,
    /// preserving the original createdAt timestamp.
    /// </summary>
    /// <param name="dryRun">When true, prints the plan without writing anything.</param>
    public void Migrate(bool dryRun)
    {
        string path = Path.Combine(this.root, "baseddd.json");

        if (!File.Exists(path))
        {
            throw new InvalidOperationException(
                $"baseddd.json not found at '{path}'. Is this a BaseDDD project?");
        }

        using JsonDocument doc = JsonDocument.Parse(File.ReadAllText(path));
        JsonElement jsonRoot = doc.RootElement;

        string previousTemplateVersion = jsonRoot.GetProperty("templateVersion").GetString()
            ?? throw new InvalidOperationException("baseddd.json is missing 'templateVersion'.");

        DateTime createdAt = jsonRoot.GetProperty("createdAt").GetDateTime();

        MigrationRunner runner = MigrationRunner.Create();

        if (dryRun)
        {
            runner.DryRun(this.root, previousTemplateVersion, this.templateVersion);
            return;
        }

        runner.Apply(this.root, previousTemplateVersion, this.templateVersion);

        FileSystem.WriteFile(
            path,
            BaseDddJsonTemplate.Generate(this.toolVersion, this.templateVersion, createdAt, DateTime.UtcNow));

        Console.WriteLine($"Project template migrated from version {previousTemplateVersion} to {this.templateVersion} with tool version {this.toolVersion}.");
    }

    /// <summary>
    /// Reads baseddd.json and checks compatibility against the current tool version.
    /// No-ops if baseddd.json does not exist (pre-versioning projects).
    /// </summary>
    public void Check()
    {
        string path = Path.Combine(this.root, "baseddd.json");

        if (!File.Exists(path))
        {
            return;
        }

        using JsonDocument doc = JsonDocument.Parse(File.ReadAllText(path));
        string templateVersion = doc.RootElement.GetProperty("templateVersion").GetString()
            ?? throw new InvalidOperationException("baseddd.json is missing 'templateVersion'.");

        CompatibilityChecker.Check(templateVersion);
    }
}
