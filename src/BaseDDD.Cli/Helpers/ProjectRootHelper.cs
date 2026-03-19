// <copyright file="ProjectRootHelper.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.Helpers;

using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

/// <summary>
/// Helper to find project's root and projects from a solution.
/// </summary>
public static class ProjectRootHelper
{
    /// <summary>
    /// Finds the project root folder by locating any solution XML file (.slnx)
    /// and returns its full path.
    /// </summary>
    /// <param name="startDirectory">Directory to start search from.</param>
    /// <returns>Root directory containing a solution file.</returns>
    public static string FindProjectRoot(string startDirectory)
    {
        DirectoryInfo? dir = new DirectoryInfo(startDirectory);

        while (dir != null)
        {
            FileInfo? solutionFile = dir.GetFiles("*.slnx").FirstOrDefault();
            if (solutionFile != null)
            {
                return dir.FullName;
            }

            dir = dir.Parent;
        }

        throw new InvalidOperationException($"Cannot find any solution file (*.slnx) from '{startDirectory}'.");
    }
}
