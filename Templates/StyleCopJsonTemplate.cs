// <copyright file="StyleCopJsonTemplate.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Templates;

/// <summary>
/// Provides stylecop.json file template.
/// </summary>
public static class StyleCopJsonTemplate
{
    /// <summary>
    /// Returns content of generated stylecop.json.
    /// </summary>
    /// <param name="owner">Repository owner.</param>
    /// <param name="license">Repository license.</param>
    /// <returns>stylecop.json file content.</returns>
    public static string Generate(string owner, string license)
    {
        int year = DateTime.UtcNow.Year;

        return $$"""
        {
            "settings": {
                "documentationRules": {
                    "companyName": "{{owner}}",
                    "copyrightText": "Copyright (c) {{year}} {{owner}}. Licensed under the {{license}} License."
                }
            }
        }
        """;
    }
}
