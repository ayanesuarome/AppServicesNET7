// <copyright file="Program.cs" company="Ayane">
// Copyright (c) Ayane. All rights reserved.
// </copyright>
namespace CodeAnalyzing;

using System.Collections.Generic;
using System.Runtime.InteropServices.JavaScript;
using CodeAnalyzing.Static_Virtual_Member_Interface;

/// <summary>
/// The main class for this console app.
/// </summary>
public class Program
{
    /// <summary>
    /// The main entry point for this console app.
    /// </summary>
    /// <param name="args">
    /// A string array of arguments passed to the console app.
    /// </param>
    public static void Main(string[] args)
    {
        Debug.WriteLine("Hello, Debugger!");

        // The Index value type is a more formal way of identifying a position, and supports counting from the end
        Index i1 = new Index(value: 3);
        Index i1a = 2; // implicit
        Index i2 = new Index(value: 2, fromEnd: true);
        Index i2a = ^2;

        // The Range value type uses Index values to indicate the start and end of its range
        Range r1 = new Range(start: new Index(3), end: new Index(7));
        Range r2 = new Range(start: 3, end: 7); // implicit
        Range r3 = 3..7;
        Range r4 = Range.StartAt(3); // from index 3 to last index
        Range r5 = 3..; // from index 3 to last index
        Range r6 = Range.EndAt(3); // from index 0 to index 3
        Range r7 = ..3; // from index 0 to index 3
        IList<string> ranges = new List<string>
        {
            "a",
            "b",
            "c",
        };

        var a = ranges[i2];
        Debug.WriteLine(a);

        // ----------------------------------------------------------- //
        ImmutablePerson person = new() { FirstName = "Ayane" };

        // person.FirstName = "Emith";  // compile error! Init-only property can be assign from object initializers
        ImmutableAnimal animal = new("name", "bird", 4);

        // animal.Species = "tiger"; // compile error! Init-only property can be assign from object initializers
        XmlDocument xmlDocument = new(); // C# 9 or later

        // Raw string literals
        Debug.WriteLine("Raw string literals");
        string xml = """
            <person age="50">
            <first_name>Mark</first_name>
            </person>
            """;
        Debug.WriteLine(xml);

        // interpolated with raw string literals
        string json = $$"""
        {
            "first_name": "{{animal.Name}}",
            "age": {{animal.Age}},
        };
        """;
        Debug.WriteLine(json);

        // Required properties in C# 11
        // Book book = new();
        Book book = new() { ISBN = "123" };

        // Static Virtual Member Interface
        Debug.WriteLine("Static Virtual Member Interface");
        RepeatSequence str = new();
        for (int i = 0; i < 2; i++)
        {
            Debug.WriteLine(str++);
        }

        // List patterns
        Debug.WriteLine("List patterns");
        int[] numbers = { 1, 2, 3 };
        Debug.WriteLine(numbers is[1, 2, 3]);  // True
        Debug.WriteLine(numbers is[1, 2, 4]);  // False
        Debug.WriteLine(numbers is[1, 2, 3, 4]);  // False
        Debug.WriteLine(numbers is[0 or 1, <= 2, >= 3]);  // True

        int[] arr = new[] { 0, 1, 2, 3 };
        Span<int> intSpan = arr;
        var otherSpan = arr.AsSpan();
    }

    /// <summary>
    /// Store.
    /// </summary>
    /// <param name="animal">Animal.</param>
    /// <param name="aux">Aux.</param>
    internal static void Store(ImmutableAnimal animal, string aux)
    {
        ArgumentNullException.ThrowIfNull(animal);
        ArgumentNullException.ThrowIfNullOrEmpty(aux);
    }

    // Pattern match Span<char> or ReadOnlySpan<char> on a constant string.

    /// <summary>
    /// Doc.
    /// </summary>
    /// <param name="s">s.</param>
    /// <returns>true/false.</returns>
    internal static bool Is123(ReadOnlySpan<char> s)
    {
        return s is "123";
    }

    /// <summary>
    /// Doc.
    /// </summary>
    /// <param name="s">s.</param>
    /// <returns>true/false.</returns>
    internal static bool IsABC(Span<char> s)
    {
        return s switch { "ABC" => true, _ => false };
    }
}