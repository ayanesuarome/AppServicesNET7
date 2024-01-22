using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Packt.Shared;

namespace Northwind.OData.Service.Controllers
{
    public class SuppliersController : ODataController
    {
        protected readonly NorthwindContext _db;

        public SuppliersController(NorthwindContext db)
        {
            _db = db;
        }

        [EnableQuery]
        public IActionResult Get()
        {
            return Ok(_db.Suppliers);
        }

        [EnableQuery]
        public IActionResult Get(int key)
        {
            return Ok(_db.Suppliers.Where(supplier => supplier.SupplierId == key));
        }
    }
}
