// <copyright file="MigrationStep_1_0_To_1_1.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Migrations;

/// <summary>
/// Placeholder migration from template 1.0 to 1.1.
/// Populate Describe() and Apply() when 1.1 is defined.
/// </summary>
internal sealed class MigrationStep_1_0_To_1_1 : IMigrationStep
{
    /// <inheritdoc/>
    public string FromVersion => "1.0";

    /// <inheritdoc/>
    public string ToVersion => "1.1";

    /// <inheritdoc/>
    public IReadOnlyList<string> Describe()
    {
        return new List<string> { "Placeholder: no changes defined yet for 1.0 → 1.1." };
    }

    /// <inheritdoc/>
    public void Apply(string root)
    {
        // No-op until 1.1 template changes are defined.
    }
}