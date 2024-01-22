using Microsoft.EntityFrameworkCore;

namespace Northwind.Console.HierarchyMapping
{
    public class HierarchyDb : DbContext
    {
        public DbSet<Person>? People { get; set; }
        public DbSet<Student>? Students { get; set; }
        public DbSet<Employee>? Employees { get; set; }

        public HierarchyDb(DbContextOptions<HierarchyDb> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=HierarchyMapping;Integrated Security=true;TrustServerCertificate=true;Connect Timeout=10");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>()
                //.UseTphMappingStrategy();
                //.UseTptMappingStrategy();
                .UseTpcMappingStrategy()
                .Property(p => p.Id)
                .HasDefaultValueSql("NEXT VALUE FOR [PersonIds]");

            modelBuilder.HasSequence<int>("PersonIds", builder => builder.StartsAt(4));
            
            modelBuilder.ApplyConfiguration(new StudentClassMapping());
            modelBuilder.ApplyConfiguration(new EmployeeClassMapping());
        }
    }
}
