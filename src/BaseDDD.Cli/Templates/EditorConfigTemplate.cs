// <copyright file="EditorConfigTemplate.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Templates;

/// <summary>
/// Provides .editorconfig file template.
/// </summary>
public static class EditorConfigTemplate
{
    /// <summary>
    /// Returns content of generated .editorconfig.
    /// </summary>
    /// <returns>.editorconfig file content.</returns>
    public static string Generate()
    {
        return """
        root = true

        [*.cs]
        dotnet_style_require_accessibility_modifiers = always:error
        dotnet_style_qualification_for_field = true:error
        csharp_style_var_for_built_in_types = false:error
        csharp_style_var_when_type_is_apparent = false:error
        csharp_style_var_elsewhere = false:error
        csharp_style_expression_bodied_methods = false:error
        csharp_style_namespace_declarations = file_scoped:error
        dotnet_diagnostic.IDE0051.severity = error
        dotnet_diagnostic.CA1822.severity = error
        """;
    }
}
