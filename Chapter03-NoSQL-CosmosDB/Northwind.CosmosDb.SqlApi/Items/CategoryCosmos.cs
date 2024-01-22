using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.CosmosDb.SqlApi.Items;

public class CategoryCosmos
{
    public int categoryId { get; set; }
    public string categoryName { get; set; } = null!;
    public string? description { get; set; }
}
