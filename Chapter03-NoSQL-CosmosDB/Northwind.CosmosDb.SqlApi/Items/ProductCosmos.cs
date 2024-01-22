using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.CosmosDb.SqlApi.Items;

public class ProductCosmos
{
    public string id { get; set; } = null!;
    public string productId { get; set; } = null!;
    public string productName { get; set; } = null!;
    public string? quantityPerUnit { get; set; }
    public decimal? unitPrice { get; set; }
    public short? unitsInStock { get; set; }
    public short? unitsOnOrder { get; set; }
    public short? reorderLevel { get; set; }
    public bool? discontinued { get; set; }
    public double? tax { get; set; }
    public CategoryCosmos? category { get; set; }
    public SupplierCosmos? supplier { get; set; }
}
