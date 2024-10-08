using GeneratingPdf;
using QuestPDF.Fluent;
using System.Diagnostics;

string filename = "catalog.pdf";

Catalog model = new()
{
    Categories = new()
    {
        new() { CategoryId = 1, CategoryName = "Beverages" },
        new() { CategoryId = 2, CategoryName = "Condiments" },
        new() { CategoryId = 3, CategoryName = "Confections" },
        new() { CategoryId = 4, CategoryName = "Dairy Products" },
        new() { CategoryId = 5, CategoryName = "Grains/Cereals" },
        new() { CategoryId = 6, CategoryName = "Meat/Poultry" },
        new() { CategoryId = 7, CategoryName = "Produce" },
        new() { CategoryId = 8, CategoryName = "Seafood" },
    }
};

CatalogDocument document = new(model);
document.GeneratePdf(filename);

// document.GeneratePdfAndShow(); No need to open with the code below

WriteLine($"PDF catalog has been created: {filename}");

try
{
    // https://docs.microsoft.com/en-us/dotnet/api/system.diagnostics.process.start
    if (OperatingSystem.IsWindows())
    {
        Process.Start(new ProcessStartInfo(filename) { UseShellExecute = true });
    }
    else if (OperatingSystem.IsMacOS())
    {
        Process.Start("open", filename);
    }
    else if (OperatingSystem.IsLinux())
    {
        Process.Start("xdg-open", filename);
    }
    else
    {
        WriteLine("Open the file manually.");
    }
}
catch (Exception ex)
{
    WriteLine($"Error opening file: {ex.Message}");
}