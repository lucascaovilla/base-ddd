// <copyright file="CompatibilityCheckerTests.cs" company="BaseDDD">
// Copyright (c) BaseDDD.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// </copyright>
namespace BaseDDD.UnitTests.Infrastructure;

using System;
using BaseDDD.Infrastructure;
using Xunit;

public class CompatibilityCheckerTests
{
    [Fact]
    public void Check_Should_Not_Throw_When_Version_Is_Current()
    {
        string current = VersionConstants.TemplateVersion;

        Exception exception = Record.Exception(() => CompatibilityChecker.Check(current));

        Assert.Null(exception);
    }

    [Fact]
    public void Check_Should_Throw_When_Project_Version_Is_Newer_Than_Tool()
    {
        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
            () => CompatibilityChecker.Check("999.0"));

        Assert.Contains("Please update the BaseDDD CLI", exception.Message);
    }

    [Fact]
    public void Check_Should_Throw_When_Project_Version_Is_Below_Minimum()
    {
        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
            () => CompatibilityChecker.Check("0.1"));

        Assert.Contains("baseddd migrate --apply", exception.Message);
    }

    [Fact]
    public void Check_Should_Throw_When_Version_Is_Unparseable()
    {
        Assert.Throws<InvalidOperationException>(
            () => CompatibilityChecker.Check("not-a-version"));
    }
}
