// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;
using Northwind.Console.HierarchyMapping;

DbContextOptionsBuilder<HierarchyDb> options = new();

using (HierarchyDb db = new(options.Options))
{
    bool deleted = await db.Database.EnsureDeletedAsync();
    WriteLine($"Database deleted: {deleted}");

    bool created = await db.Database.EnsureCreatedAsync();
    WriteLine($"Database created: {created}");

    WriteLine("SQL script used to create the database:");
    WriteLine(db.Database.GenerateCreateScript());

    if (db.Students is null || !db.Students.Any())
    {
        WriteLine("There are no students.");
    }
    else
    {
        foreach (Student student in db.Students)
        {
            WriteLine("{0} studies {1}", student.Name, student.Subject);
        }
    }

    if (db.Employees is null || !db.Employees.Any())
    {
        WriteLine("There are no employees.");
    }
    else
    {
        foreach (Employee employee in db.Employees)
        {
            WriteLine("{0} was hired on {1}", employee.Name, employee.HireDate);
        }
    }

    if(db.People is null || !db.People.Any())
    {
        WriteLine("There are no people.");
    }
    else
    {
        foreach (Person person in db.People)
        {
            WriteLine("{0} has ID of {1}", person.Name, person.Id);
        }
    }
}