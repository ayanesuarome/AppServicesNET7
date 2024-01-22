// <copyright file="ImmutableAnimal.cs" company="Ayane">
// Copyright (c) Ayane. All rights reserved.
// </copyright>
namespace CodeAnalyzing;

/// <summary>
/// Record must be initialized with all its Init properties and can't be edited later.
/// </summary>
internal record ImmutableAnimal(string Name, string Species, int Age);