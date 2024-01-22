using Gremlin.Net.Driver;

namespace Northwind.CosmosDb.Gremlin;

partial class Program
{
    private static async Task Main(string[] args)
    {
        await CreateCosmosGraphResources();

        SectionTitle("Gremlin Server details:");
        WriteLine($"  Uri:      {gremlinServer.Uri}");
        WriteLine($"  Username: {gremlinServer.Username}");
        WriteLine($"  Password: {gremlinServer.Password}");

        await CreateProductVertices();
        await CreateCustomerVertices();
    }
}