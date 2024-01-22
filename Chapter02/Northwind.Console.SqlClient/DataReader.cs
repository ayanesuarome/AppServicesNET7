using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Northwind.Console.SqlClient
{
    public class DataReader
    {
        public DataReader(SqlConnection connection)
        {
            this.connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        private SqlConnection connection { get; set; }
        private SqlDataReader? reader { get; set; }

        private async Task CloseReaderAsync()
        {
            ArgumentNullException.ThrowIfNull(reader);
            await this.reader.CloseAsync();
        }

        public async Task GetProductsAsync()
        {
            Write("Enter a unit price: ");
            string? priceText = ReadLine();

            if (!decimal.TryParse(priceText, out decimal price))
            {
                WriteLine("You must enter a valid unit price.");
                return;
            }

            SqlCommand cmd = connection.CreateCommand();

            WriteLine("Execute command using:");
            WriteLine(" 1 - Text");
            WriteLine(" 2 - Stored Procedure");
            WriteLine();
            Write("Press a key: ");
            ConsoleKey key = ReadKey().Key;
            WriteLine(); WriteLine();

            SqlParameter p1, p2 = new(), p3 = new();

            if (key is ConsoleKey.D1 or ConsoleKey.NumPad1)
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "SELECT ProductId, ProductName, UnitPrice FROM Products WHERE UnitPrice > @price";
                cmd.Parameters.AddWithValue("price", price);
            }
            else if (key is ConsoleKey.D2 or ConsoleKey.NumPad2)
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetExpensiveProducts";

                p1 = new()
                {
                    ParameterName = "price",
                    SqlDbType = SqlDbType.Money,
                    SqlValue = price
                };

                p2 = new()
                {
                    Direction = ParameterDirection.Output,
                    ParameterName = "count",
                    SqlDbType = SqlDbType.Int,
                };

                p3 = new()
                {
                    Direction = ParameterDirection.ReturnValue,
                    ParameterName = "rv",
                    SqlDbType = SqlDbType.Int
                };

                cmd.Parameters.Add(p1);
                cmd.Parameters.Add(p2);
                cmd.Parameters.Add(p3);
            }
            
            reader = await cmd.ExecuteReaderAsync();

            WriteLine("----------------------------------------------------------");
            WriteLine("| {0,5} | {1,-35} | {2,8} |", "Id", "Name", "Price");
            WriteLine("----------------------------------------------------------");

            while (await reader.ReadAsync())
            {
                WriteLine("| {0,5} | {1,-35} | {2,8:C} |",
                //reader.GetInt32("ProductId"),
                //reader.GetString("ProductName"),
                //reader.GetDecimal("UnitPrice"));
                await reader.GetFieldValueAsync<int>("ProductId"),
                await reader.GetFieldValueAsync<string>("ProductName"),
                await reader.GetFieldValueAsync<decimal>("UnitPrice"));
            }

            WriteLine("----------------------------------------------------------");

            // If a stored procedure returns result sets as well as parameters, then the data reader
            // for the result sets must be closed before the parameters can be read.
            await CloseReaderAsync();

            if (key is ConsoleKey.D2 or ConsoleKey.NumPad2)
            {
                WriteLine($"Output count: {p2.Value}");
                WriteLine($"Return value: {p3.Value}");
            }
        }

        public async Task InjectSqlAttackAsync()
        {
            Write("Enter a unit price: ");
            // Attack values:
            // 1. ' and ProductId=1;--  It closes the where clause and looks for product ID 1 and comment out the rest of the statement
            // 2. '; DROP TABLE [TableName];--  It closes the where clause and delete all the products
            string? priceText = ReadLine();

            SqlCommand cmd = connection.CreateCommand();

            WriteLine("Execute command using:");
            WriteLine(" 1 - Text");
            WriteLine(" 2 - Stored Procedure");
            WriteLine();
            Write("Press a key: ");
            ConsoleKey key = ReadKey().Key;
            WriteLine(); WriteLine();

            if (key is ConsoleKey.D1 or ConsoleKey.NumPad1)
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = $"SELECT ProductId, ProductName, UnitPrice FROM Products WHERE UnitPrice > '{priceText}'";
            }

            reader = await cmd.ExecuteReaderAsync();

            WriteLine("----------------------------------------------------------");
            WriteLine("| {0,5} | {1,-35} | {2,8} |", "Id", "Name", "Price");
            WriteLine("----------------------------------------------------------");

            while (await reader.ReadAsync())
            {
                WriteLine("| {0,5} | {1,-35} | {2,8:C} |",
                //reader.GetInt32("ProductId"),
                //reader.GetString("ProductName"),
                //reader.GetDecimal("UnitPrice"));
                await reader.GetFieldValueAsync<int>("ProductId"),
                await reader.GetFieldValueAsync<string>("ProductName"),
                await reader.GetFieldValueAsync<decimal>("UnitPrice"));
            }

            WriteLine("----------------------------------------------------------");

            // If a stored procedure returns result sets as well as parameters, then the data reader
            // for the result sets must be closed before the parameters can be read.
            await CloseReaderAsync();
        }

        public async Task GetSuppliersAsync()
        {
            IEnumerable<Supplier> suppliers = await connection.QueryAsync<Supplier>(
                sql: "SELECT * FROM Suppliers WHERE Country=@Country",
                param: new { Country = "Germany" });
            
            foreach (Supplier supplier in suppliers)
            {
                WriteLine("{0}: {1}, {2}, {3}",
                supplier.SupplierId,
                supplier.CompanyName,
                supplier.City,
                supplier.Country);
            }

            // Using DynamicParameters
            DynamicParameters queryParams = new();

            queryParams.Add("Country", "Germany", DbType.String);
            IEnumerable<Supplier> suppliers1 = await connection.QueryAsync<Supplier>(
                sql: $"SELECT * FROM Suppliers WHERE Country = @Country", queryParams);

            foreach (Supplier supplier in suppliers1)
            {
                WriteLine("{0}: {1}, {2}, {3}",
                supplier.SupplierId,
                supplier.CompanyName,
                supplier.City,
                supplier.Country);
            }
        }
    }
}
