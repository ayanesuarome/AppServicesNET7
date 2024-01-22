using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Northwind.Console.HierarchyMapping
{
    public class EmployeeClassMapping : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            var json = File.ReadAllText(@"seed_employees.json");
            var employees = JsonConvert.DeserializeObject<IEnumerable<Employee>>(json) ?? Array.Empty<Employee>();

            builder.HasData(employees);
        }
    }
}
