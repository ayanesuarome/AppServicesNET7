namespace Northwind.CosmosDb.SqlApi;

partial class Program
{
    private static async Task Main(string[] args)
    {
        await CreateCosmosResourcesAsync();
        //await CreateProductItemsAsync();
        //await ListProductItemsAsync("SELECT p.id, p.productName, p.unitPrice FROM Items p WHERE p.category.categoryName = 'Beverages'");
        await ListProductItemsAsync("SELECT p.id, p.productName, p.unitPrice, udf.salesTax(p.unitPrice) AS tax FROM Items p WHERE p.category.categoryName = 'Beverages'");
        //await DeleteProductItemsAsync();
    }
}