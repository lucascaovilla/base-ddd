// <copyright file="MigrationRunner.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Infrastructure;

using Olav.Migrations;

/// <summary>
/// Chains and executes <see cref="IMigrationStep"/> instances to bring a project
/// from its current template version up to the tool's current template version.
/// </summary>
public sealed class MigrationRunner
{
    private readonly IReadOnlyList<IMigrationStep> steps;

    private MigrationRunner(IReadOnlyList<IMigrationStep> steps)
    {
        this.steps = steps;
    }

    /// <summary>
    /// Creates a runner pre-registered with all known migration steps.
    /// </summary>
    /// <returns>List of known migrations.</returns>
    public static MigrationRunner Create()
    {
        return new ([new MigrationStep_1_0_To_1_1()]);
    }

    /// <summary>
    /// Prints what would change without applying anything.
    /// </summary>
    /// <param name="root">Project root path.</param>
    /// <param name="fromVersion">Current template version of the project.</param>
    /// <param name="toVersion">Target template version.</param>
    public void DryRun(string root, string fromVersion, string toVersion)
    {
        List<IMigrationStep> chain = this.BuildChain(fromVersion, toVersion);

        if (chain.Count == 0)
        {
            Console.WriteLine($"Project is already at template version {toVersion}. Nothing to migrate.");
            return;
        }

        Console.WriteLine($"Dry run: {fromVersion} → {toVersion}");
        Console.WriteLine();

        foreach (IMigrationStep step in chain)
        {
            Console.WriteLine($"  [{step.FromVersion} → {step.ToVersion}]");
            foreach (string line in step.Describe())
            {
                Console.WriteLine($"    - {line}");
            }

            Console.WriteLine();
        }

        Console.WriteLine("Run 'olav migrate --apply' to apply these changes.");
    }

    /// <summary>
    /// Applies all required migration steps in order.
    /// olav.json is written by the caller (VersionEnforcementGenerator) after this returns.
    /// </summary>
    /// <param name="root">Project root path.</param>
    /// <param name="fromVersion">Current template version of the project.</param>
    /// <param name="toVersion">Target template version.</param>
    public void Apply(string root, string fromVersion, string toVersion)
    {
        List<IMigrationStep> chain = this.BuildChain(fromVersion, toVersion);

        if (chain.Count == 0)
        {
            Console.WriteLine($"Project is already at template version {toVersion}. Nothing to migrate.");
            return;
        }

        foreach (IMigrationStep step in chain)
        {
            Console.WriteLine($"  Applying [{step.FromVersion} → {step.ToVersion}]...");
            step.Apply(root);
        }
    }

    private List<IMigrationStep> BuildChain(string fromVersion, string toVersion)
    {
        List<IMigrationStep> chain = new List<IMigrationStep>();
        string current = fromVersion;

        while (current != toVersion)
        {
            IMigrationStep step = this.steps.FirstOrDefault(s => s.FromVersion == current)
                ?? throw new InvalidOperationException(
                    $"No migration step found from '{current}'. " +
                    $"Migration chain is broken — please update the Olav CLI.");

            chain.Add(step);
            current = step.ToVersion;
        }

        return chain;
    }
}
