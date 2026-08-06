// <copyright file="Program.cs" company="ING">
// Copyright (c) ING. All rights reserved.
// </copyright>

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace WorkingWithCultures;

public partial class Program
{
    public static void Main(string[] args)
    {
        using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services.AddLocalization(opts => opts.ResourcesPath = "Resources");
                    services.AddTransient<PacktResources>();
                }).Build();

        Cultures(host);
        //foreach (var doc in PriceCulture())
        //{
        //    WriteLine(doc);
        //}
    }

    private static void Cultures(IHost host)
    {
        // to enable special characters like €
        OutputEncoding = System.Text.Encoding.Unicode;

        OutputCultures("Current culture");

        WriteLine("Example ISO culture codes:");
        string[] cultureCodes = new[] {
            "da-DK", "en-GB", "en-US", "fa-IR", "fr-CA",
            "fr-FR", "he-IL", "pl-PL", "sl-SI", "bg-BG"
        };

        foreach (string code in cultureCodes)
        {
            CultureInfo culture = CultureInfo.GetCultureInfo(code);
            WriteLine(" {0}: {1} / {2}", culture.Name, culture.EnglishName, culture.NativeName);

        }

        WriteLine();

        Write("Enter an ISO culture code: ");
        string? cultureCode = ReadLine();
        if (string.IsNullOrWhiteSpace(cultureCode))
        {
            cultureCode = "en-US";
        }

        CultureInfo ci;

        try
        {
            ci = CultureInfo.GetCultureInfo(cultureCode);
        }
        catch (CultureNotFoundException)
        {
            WriteLine($"Culture code not found: {cultureCode}");
            WriteLine("Exiting the app.");
            return;
        }

        // change the current cultures on the thread
        CultureInfo.CurrentCulture = ci;
        CultureInfo.CurrentUICulture = ci;
        OutputCultures("After changing the current culture");

        PacktResources resources = host.Services.GetRequiredService<PacktResources>();

        Write(resources.GetEnterYourNamePrompt());
        string? name = ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            name = "Bob";
        }
        Write(resources.GetEnterYourDobPrompt());
        string? dobText = ReadLine();

        if (string.IsNullOrWhiteSpace(dobText))
        {
            // if they do not enter a DOB then use sensible defaults for their culture
            dobText = ci.Name switch
            {
                "en-US" or "fr-CA" => "1/27/1990",
                "da-DK" or "fr-FR" or "pl-PL" => "27/1/1990",
                "fa-IR" => "1990/1/27",
                _ => "1/27/1990"
            };
        }

        Write(resources.GetEnterYourSalaryPrompt());
        string? salaryText = ReadLine();
        if (string.IsNullOrWhiteSpace(salaryText))
        {
            salaryText = "34500";
        }

        DateTime dob = DateTime.Parse(dobText);
        int minutes = (int)DateTime.Today.Subtract(dob).TotalMinutes;
        decimal salary = decimal.Parse(salaryText);
        WriteLine(resources.GetPersonDetails(name, dob, minutes, salary));
    }

    private static string[] PriceCulture()
    {
        Write("Enter an ISO culture code: ");
        string? cultureCode = ReadLine();
        if (string.IsNullOrWhiteSpace(cultureCode))
        {
            cultureCode = "en-US";
        }

        CultureInfo ci;

        try
        {
            ci = CultureInfo.GetCultureInfo(cultureCode);
        }
        catch (CultureNotFoundException)
        {
            WriteLine($"Culture code not found: {cultureCode}");
            WriteLine("Exiting the app.");
            return new string[0];
        }

        // change the current cultures on the thread
        CultureInfo.CurrentCulture = ci;
        CultureInfo.CurrentUICulture = ci;

        decimal price = 54321.99M;
        string document = $$"""
        {
            "price": "{{price:N2}}"
        }
        """;

        string documentInvariantCulture = $$"""
        {
            "price": "{{price.ToString("N2", CultureInfo.InvariantCulture)}}"
        }
        """;

        return new[] {document, documentInvariantCulture};
    }
}
