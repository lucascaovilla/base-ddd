// <copyright file="DirectoryBuildPropsTemplate.cs" company="Olav">
// Copyright (c) Olav.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace Olav.Templates;

/// <summary>
/// Provides Directory.Build.props file template.
/// </summary>
public static class DirectoryBuildPropsTemplate
{
    /// <summary>
    /// Returns content of generated Directory.Build.props.
    /// </summary>
    /// <returns>Directory.Build.props file content.</returns>
    public static string Generate()
    {
        return """
        <Project>
            <PropertyGroup>
                <TreatWarningsAsErrors>true</TreatWarningsAsErrors>
                <Nullable>enable</Nullable>
                <ImplicitUsings>enable</ImplicitUsings>
                <AnalysisLevel>latest</AnalysisLevel>
                <EnforceCodeStyleInBuild>true</EnforceCodeStyleInBuild>
                <GenerateDocumentationFile>true</GenerateDocumentationFile>
                <NoWarn>$(NoWarn);1591</NoWarn>
            </PropertyGroup>

            <ItemGroup>
                <AdditionalFiles Include="$(MSBuildThisFileDirectory)stylecop.json" />
            </ItemGroup>
        </Project>
        """;
    }
}
