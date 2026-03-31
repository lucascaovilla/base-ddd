// <copyright file="OlavJsonTemplate.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Templates;

/// <summary>
/// Provides olav.json template.
/// </summary>
public static class OlavJsonTemplate
{
    /// <summary>
    /// Returns content of generated olav.json file.
    /// </summary>
    /// <param name="toolVersion">Olav tool current version.</param>
    /// <param name="templateVersion">Olav written template version.</param>
    /// <param name="createdAt">Datetime of generation of the project.</param>
    /// <param name="updatedAt">Datetime of last update on the project by Olav tool.</param>
    /// <returns>olav.json file content.</returns>
    public static string Generate(
        string toolVersion,
        string templateVersion,
        DateTime createdAt,
        DateTime updatedAt)
    {
        string createdAtIso = createdAt.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");
        string updatedAtIso = updatedAt.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ");

        return $$"""
                {
                  "toolVersion": "{{toolVersion}}",
                  "templateVersion": "{{templateVersion}}",
                  "createdAt": "{{createdAtIso}}",
                  "updatedAt": "{{updatedAtIso}}"
                }
                """;
    }
}
