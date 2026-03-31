// <copyright file="FileHeaderTemplate.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Templates;

/// <summary>
/// Provides common file header generation.
/// </summary>
public static class FileHeaderTemplate
{
    /// <summary>
    /// Generates the copyright header used by all generated files.
    /// </summary>
    /// <param name="fileName">Repository fileName.</param>
    /// <param name="owner">Repository owner.</param>
    /// <param name="license">Repository license.</param>
    /// <returns>Generated file header.</returns>
    public static string Generate(string fileName, string owner, string license)
    {
        int year = DateTime.UtcNow.Year;

        return $$"""
        // <copyright file="{{fileName}}" company="{{owner}}">
        // Copyright (c) {{year}} {{owner}}. Licensed under the {{license}} License.
        // </copyright>

        """;
    }
}
