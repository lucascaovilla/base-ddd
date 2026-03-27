// <copyright file="BaseDddJsonTemplate.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Templates;

/// <summary>
/// Provides baseddd.json template.
/// </summary>
public static class BaseDddJsonTemplate
{
    /// <summary>
    /// Returns content of generated baseddd.json file.
    /// </summary>
    /// <param name="toolVersion">BaseDDD tool current version.</param>
    /// <param name="templateVersion">BaseDDD written template version.</param>
    /// <param name="createdAt">Datetime of generation of the project.</param>
    /// <param name="updatedAt">Datetime of last update on the project by BaseDDD tool.</param>
    /// <returns>baseddd.json file content.</returns>
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
