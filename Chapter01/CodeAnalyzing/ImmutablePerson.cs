// <copyright file="ImmutablePerson.cs" company="Ayane">
// Copyright (c) Ayane. All rights reserved.
// </copyright>
namespace CodeAnalyzing;

/// <summary>
/// Class.
/// </summary>
internal class ImmutablePerson
{
    /// <summary>
    /// Gets Name.
    /// Init only setter.
    /// </summary>
    public string? FirstName { get; init; }
}