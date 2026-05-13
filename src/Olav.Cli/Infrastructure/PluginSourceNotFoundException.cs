// <copyright file="PluginSourceNotFoundException.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Infrastructure;

/// <summary>
/// Thrown by <see cref="PluginSourceResolver"/> when a source alias cannot be resolved
/// via local sources, global sources, or the built-in registry.
/// Extends <see cref="InvalidOperationException"/> so existing catch blocks remain compatible.
/// </summary>
public class PluginSourceNotFoundException : InvalidOperationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PluginSourceNotFoundException"/> class.
    /// </summary>
    /// <param name="alias">The alias that could not be resolved.</param>
    public PluginSourceNotFoundException(string alias)
        : base("✗ Unknown source '" + alias + "'. Run: olav source add " + alias + " <url>")
    {
        this.Alias = alias;
    }

    /// <summary>Gets the alias that could not be resolved.</summary>
    public string Alias { get; }
}
