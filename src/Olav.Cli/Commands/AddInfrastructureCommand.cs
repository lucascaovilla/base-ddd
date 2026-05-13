// <copyright file="AddInfrastructureCommand.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Commands;

using Olav.Generation;
using Olav.Helpers;

/// <summary>
/// Handles <c>olav add infrastructure &lt;source&gt; [--paramName value …]</c>.
/// Resolves the plugin, prompts interactively for any required parameters not
/// supplied on the command line, then delegates to <see cref="PluginInstallGenerator"/>.
/// </summary>
public static class AddInfrastructureCommand
{
    /// <summary>
    /// Executes the <c>add infrastructure</c> command.
    /// </summary>
    /// <param name="args">Full argument list (including the leading <c>add infrastructure</c> tokens).</param>
    public static void Execute(string[] args)
    {
        if (args.Length < 3)
        {
            Console.WriteLine("Usage: olav add infrastructure <source> [--paramName value …]");
            return;
        }

        string rawSource = args[2];
        string root = ProjectRootHelper.FindProjectRoot(Directory.GetCurrentDirectory());

        try
        {
            PluginInstallGenerator installer = PluginInstallGenerator.Create(root);

            PluginInfoResult info = installer.FetchInfo(rawSource);

            Dictionary<string, string> parameters = ParseFlags(args, 3);
            PromptMissingParameters(info, parameters);

            installer.Install(rawSource, parameters);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    /// <summary>
    /// Prompts the user interactively for any required parameters not supplied via CLI flags.
    /// Only parameters with no <c>default</c> in the manifest and not already in
    /// <paramref name="parameters"/> trigger a prompt.
    /// </summary>
    /// <param name="info">Plugin metadata including parameter definitions.</param>
    /// <param name="parameters">Pre-populated parameters from CLI flags; modified in place.</param>
    internal static void PromptMissingParameters(
        PluginInfoResult info,
        Dictionary<string, string> parameters)
    {
        foreach (PluginParameterDefinition param in info.Parameters)
        {
            if (!parameters.ContainsKey(param.Name) && param.Default == null)
            {
                Console.Write("Enter " + param.Name + ": ");
                string value = Console.ReadLine() ?? string.Empty;
                parameters[param.Name] = value;
            }
        }
    }

    private static Dictionary<string, string> ParseFlags(string[] args, int startIndex)
    {
        Dictionary<string, string> result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        for (int i = startIndex; i < args.Length - 1; i++)
        {
            if (args[i].StartsWith("--", StringComparison.Ordinal))
            {
                string key = args[i].Substring(2);
                result[key] = args[i + 1];
                i++;
            }
        }

        return result;
    }
}
