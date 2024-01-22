// See https://aka.ms/new-console-template for more information

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Northwind.Console.EFCore;

SqlConnectionStringBuilder builder = new();

builder.InitialCatalog = "Northwind";
builder.MultipleActiveResultSets = true;
builder.Encrypt = true;
builder.TrustServerCertificate = true;
builder.ConnectTimeout = 10;
builder.DataSource = "."; // Local SQL Server
builder.IntegratedSecurity = true;

DbContextOptionsBuilder<NorthwindDbContext> options = new();
options.UseSqlServer(builder.ConnectionString);

using (NorthwindDbContext db = new(options.Options))
{
    Write("Enter a unit price: ");
    string? priceText = ReadLine();
    if (!decimal.TryParse(priceText, out decimal price))
    {
        WriteLine("You must enter a valid unit price.");
        return;
    }

    // We have to use var because we are projecting into an anonymous type.
    var products = db.Products
    .Where(p => p.UnitPrice > price)
    .Select(p => new { p.ProductId, p.ProductName, p.UnitPrice });

    WriteLine("----------------------------------------------------------");
    WriteLine("| {0,5} | {1,-35} | {2,8} |", "Id", "Name", "Price");
    WriteLine("----------------------------------------------------------");
    foreach (var p in products)
    {
        WriteLine("| {0,5} | {1,-35} | {2,8:C} |",
        p.ProductId, p.ProductName, p.UnitPrice);
    }
    WriteLine("----------------------------------------------------------");
    WriteLine(products.ToQueryString());
    WriteLine();
    WriteLine($"Provider: {db.Database.ProviderName}");
    WriteLine($"Connection: {db.Database.GetConnectionString()}");
}