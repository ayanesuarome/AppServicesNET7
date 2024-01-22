﻿using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Northwind.CosmosDb.SqlApi.Items;
using Packt.Shared;
using System.Net;
using System.Runtime.CompilerServices;

namespace Northwind.CosmosDb.SqlApi
{
    partial class Program
    {
        // to use Azure Cosmos DB in the local emulator
        private static string endpointUri = "https://localhost:8081";
        private static string primaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

        // to use Azure Cosmos DB in the cloud
        //private static string account = "cosmosdb-sayane"; // your account
        //private static string endpointUri = $"https://{account}.documents.azure.com:443/";
        //private static string primaryKey = "bSor7z1DLtoHsI3XHsEYAtzd2hE4j0LNYlJArfOvQQwF9ehazkBqeVrAGOKFZ4QF0emO1gkukNP2ACDbqLZ2Tg==";

        private const string DatabaseId = "Northwind";
        private const string ProducContainer = "Products";

        static async Task CreateCosmosResourcesAsync()
        {
            SectionTitle("Creating Cosmos resources");

            try
            {
                using CosmosClient client = new(accountEndpoint: endpointUri, authKeyOrResourceToken: primaryKey);
                DatabaseResponse dbResponse = await client.CreateDatabaseIfNotExistsAsync("Northwind", throughput: 400 /* RU/s */);
                
                string status = dbResponse.StatusCode switch
                {
                    HttpStatusCode.OK => "exists",
                    HttpStatusCode.Created => "created",
                    _ => "unknown",
                };

                WriteLine("Database Id: {0}, Status: {1}.", arg0: dbResponse.Database.Id, arg1: status);
                
                IndexingPolicy indexingPolicy = new()
                {
                    IndexingMode = IndexingMode.Consistent,
                    Automatic = true, // items are indexed unless explicitly excluded
                    IncludedPaths = { new IncludedPath { Path = "/*" } }
                };

                ContainerProperties containerProperties = new("Products", partitionKeyPath: "/productId")
                {
                    IndexingPolicy = indexingPolicy
                };
                ContainerResponse containerResponse = await dbResponse.Database
                    .CreateContainerIfNotExistsAsync(containerProperties, throughput: 600 /* RU/s */);
                
                status = dbResponse.StatusCode switch
                {
                    HttpStatusCode.OK => "exists",
                    HttpStatusCode.Created => "created",
                    _ => "unknown",
                };

                WriteLine("Container Id: {0}, Status: {1}.", arg0: containerResponse.Container.Id, arg1: status);
                Container container = containerResponse.Container;
                ContainerProperties properties = await container.ReadContainerAsync();
                WriteLine($" PartitionKeyPath: {properties.PartitionKeyPath}");
                WriteLine($" LastModified: {properties.LastModified}");
                WriteLine(" IndexingPolicy.IndexingMode: {0}", arg0: properties.IndexingPolicy.IndexingMode);
                WriteLine(" IndexingPolicy.IncludedPaths: {0}",
                    arg0: string.Join(",", properties.IndexingPolicy.IncludedPaths.Select(path => path.Path)));
                WriteLine($" IndexingPolicy: {properties.IndexingPolicy}");
            }
            catch (HttpRequestException ex)
            {
                WriteLine("Error: {0}", arg0: ex.Message);
                WriteLine("Hint: Make sure the Azure Cosmos Emulator is running.");
            }
            catch (Exception ex)
            {
                WriteLine("Error: {0} says {1}",
                arg0: ex.GetType(),
                arg1: ex.Message);
            }
        }

        static async Task CreateProductItemsAsync()
        {
            SectionTitle("Create product items");
            double totalCharge = 0.0;

            try
            {
                using CosmosClient client = new(accountEndpoint: endpointUri, authKeyOrResourceToken: primaryKey);
                Container container = client.GetContainer(databaseId: DatabaseId, containerId: ProducContainer);

                using NorthwindContext db = new();
                ProductCosmos[] products = db.Products
                    // get the related data for embedding
                    .Include(p => p.Category)
                    .Include(p => p.Supplier)
                    // filter any products with null category or supplier to avoid null warnings
                    .Where(p => (p.Category != null && p.Supplier != null))
                    // project the EF Core entities into Cosmos JSON types
                    .Select(p => new ProductCosmos
                    {
                        id = p.ProductId.ToString(),
                        productId = p.ProductId.ToString(),
                        productName = p.ProductName,
                        quantityPerUnit = p.QuantityPerUnit,
                        category = new CategoryCosmos
                        {
                            categoryId = p.Category!.CategoryId,
                            categoryName = p.Category.CategoryName,
                            description = p.Category.Description
                        },
                        supplier = new SupplierCosmos
                        {
                            supplierId = p.Supplier!.SupplierId,
                            companyName = p.Supplier.CompanyName,
                            contactName = p.Supplier.ContactName,
                            contactTitle = p.Supplier.ContactTitle,
                            address = p.Supplier.Address,
                            city = p.Supplier.City,
                            country = p.Supplier.Country,
                            postalCode = p.Supplier.PostalCode,
                            region = p.Supplier.Region,
                            phone = p.Supplier.Phone,
                            fax = p.Supplier.Fax,
                            homePage = p.Supplier.HomePage
                        },
                        unitPrice = p.UnitPrice,
                        unitsInStock = p.UnitsInStock,
                        reorderLevel = p.ReorderLevel,
                        unitsOnOrder = p.UnitsOnOrder,
                        discontinued = p.Discontinued
                    })
                    .ToArray();

                foreach (ProductCosmos product in products)
                {
                    try
                    {
                        ItemResponse<ProductCosmos> productResponse =
                            await container.ReadItemAsync<ProductCosmos>(
                                product.id,
                                new PartitionKey(product.productId));

                        WriteLine("Item with id: {0} exists. Query consumed {1} RUs.", productResponse.Resource.id, productResponse.RequestCharge);
                        totalCharge += productResponse.RequestCharge;
                    }
                    catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
                    {
                        ItemResponse<ProductCosmos> productResponse = await container.CreateItemAsync(product);
                        WriteLine("Created item with id: {0}. Insert consumed {1} RUs.", productResponse.Resource.id, productResponse.RequestCharge);
                        totalCharge += productResponse.RequestCharge;
                    }
                    catch (Exception ex)
                    {
                        WriteLine("Error: {0} says {1}", arg0: ex.GetType(), arg1: ex.Message);
                    }
                }
            }
            catch(HttpRequestException ex)
            {
                WriteLine("Error: {0}", arg0: ex.Message);
                WriteLine("Hint: Make sure the Azure Cosmos Emulator is running.");
            }
            catch(Exception ex)
            {
                WriteLine("Error: {0} says {1}", arg0: ex.GetType(), arg1: ex.Message);
            }

            WriteLine("Total requests charge: {0:N2} RUs", totalCharge);
        }

        static async Task ListProductItemsAsync(string sqlText = "SELECT * FROM c")
        {
            SectionTitle("Listing product items");

            try
            {
                using CosmosClient client = new(accountEndpoint: endpointUri, authKeyOrResourceToken: primaryKey);
                Container container = client.GetContainer(databaseId: DatabaseId, containerId: ProducContainer);
                
                WriteLine("Running query: {0}", sqlText);
                QueryDefinition query = new(sqlText);
                using FeedIterator<ProductCosmos> resultsIterator = container.GetItemQueryIterator<ProductCosmos>(query);

                if (!resultsIterator.HasMoreResults)
                {
                    WriteLine("No results found.");
                }

                while(resultsIterator.HasMoreResults)
                {
                    FeedResponse<ProductCosmos> products = await resultsIterator.ReadNextAsync();
                    WriteLine("Status code: {0}, Request charge: {1} RUs.", products.StatusCode, products.RequestCharge);
                    WriteLine("{0} products found.", arg0: products.Count);

                    foreach (ProductCosmos product in products)
                    {
                        WriteLine("id: {0}, productName: {1}, unitPrice: {2}, tax: {3}",
                            product.id,
                            product.productName,
                            product.unitPrice,
                            product.tax);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                WriteLine("Error: {0}", arg0: ex.Message);
                WriteLine("Hint: Make sure the Azure Cosmos Emulator is running.");
            }
            catch (Exception ex)
            {
                WriteLine("Error: {0} says {1}", arg0: ex.GetType(), arg1: ex.Message);
            }
        }

        static async Task DeleteProductItemsAsync()
        {
            double totalCharge = 0.0;

            try
            {
                using CosmosClient client = new(accountEndpoint: endpointUri, authKeyOrResourceToken: primaryKey);
                Container container = client.GetContainer(databaseId: DatabaseId, containerId: ProducContainer);

                string sqlText = "SELECT * FROM c";
                WriteLine("Running query: {0}", sqlText);
                QueryDefinition query = new(sqlText);
                using FeedIterator<ProductCosmos> resultsIterator = container.GetItemQueryIterator<ProductCosmos>(query);

                while(resultsIterator.HasMoreResults)
                {
                    FeedResponse<ProductCosmos> products = await resultsIterator.ReadNextAsync();

                    foreach (ProductCosmos product in products)
                    {
                        WriteLine("Delete id: {0}, productName: {1}", arg0: product.id, arg1: product.productName);
                        ItemResponse<ProductCosmos> response = await container.DeleteItemAsync<ProductCosmos>(
                            id: product.id,
                            partitionKey: new PartitionKey(product.productId));

                        WriteLine("Status code: {0}, Request charge: {1} RUs.", response.StatusCode, response.RequestCharge);

                        totalCharge += response.RequestCharge;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                WriteLine("Error: {0}", arg0: ex.Message);
                WriteLine("Hint: Make sure the Azure Cosmos Emulator is running.");
            }
            catch (Exception ex)
            {
                WriteLine("Error: {0} says {1}", arg0: ex.GetType(), arg1: ex.Message);
            }

            WriteLine("Total requests charge: {0:N2} RUs", totalCharge);
        }
    }
}
