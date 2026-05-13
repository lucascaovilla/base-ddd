// <copyright file="StringExtensions.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Extensions;

using System.Text.RegularExpressions;

/// <summary>
/// Extension methods for <see cref="string"/>.
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Converts a PascalCase or camelCase string to dash-case (kebab-case).
    /// Example: <c>MyProject</c> → <c>my-project</c>.
    /// </summary>
    /// <param name="input">The string to convert.</param>
    /// <returns>The dash-case equivalent, or the original value if null or empty.</returns>
    public static string ToDashCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        string result = Regex.Replace(input, "(?<!^)([A-Z])", "-$1");
        return result.ToLowerInvariant();
    }
}
