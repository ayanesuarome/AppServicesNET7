// <copyright file="Book.cs" company="Ayane">
// Copyright (c) Ayane. All rights reserved.
// </copyright>
#nullable enable

namespace CodeAnalyzing;

/// <summary>
/// Class.
/// </summary>
internal class Book
{
    /// <summary>
    /// Gets or sets ISBN.
    /// </summary>
    required public string ISBN { get; set; }

    /// <summary>
    /// Gets or sets Title.
    /// </summary>
    public string? Title { get; set; }
}