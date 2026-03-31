// <copyright file="DirectoryPackagePropsTemplate.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Templates;

/// <summary>
/// Provides Directory.Package.props file template.
/// </summary>
public static class DirectoryPackagePropsTemplate
{
    /// <summary>
    /// Returns content of generated Directory.Package.props.
    /// </summary>
    /// <returns>Directory.Package.props file content.</returns>
    public static string Generate()
    {
        return """
        <Project>
            <ItemGroup>
            <PackageVersion Include="StyleCop.Analyzers" Version="1.2.0-beta.507" />
            <PackageVersion Include="coverlet.collector" Version="6.0.0" />
            </ItemGroup>
        </Project>
        """;
    }
}
