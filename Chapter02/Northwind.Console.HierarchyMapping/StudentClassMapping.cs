using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Northwind.Console.HierarchyMapping
{
    public class StudentClassMapping : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            var json = File.ReadAllText(@"seed_students.json");
            var students = JsonConvert.DeserializeObject<IEnumerable<Student>>(json) ?? new List<Student>();
            builder.HasData(students);
        }
    }
}
