// <copyright file="InstalledPluginHelper.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Helpers;

using Olav.Infrastructure;

/// <summary>
/// Provides plugin detection helpers for <c>add</c> subcommands.
/// </summary>
public static class InstalledPluginHelper
{
    /// <summary>
    /// Returns the id of the first installed infrastructure DB plugin found in <c>olav.json</c>,
    /// or <see langword="null"/> if no such plugin is installed.
    /// </summary>
    /// <param name="root">Absolute path to the project root containing <c>olav.json</c>.</param>
    /// <returns>Plugin id string, or <see langword="null"/>.</returns>
    public static string? ResolveInfrastructurePlugin(string root)
    {
        return OlavConfig.Load(root).ResolveInfrastructurePlugin();
    }
}
