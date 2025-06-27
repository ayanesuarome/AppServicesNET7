using System.Reflection;
using WorkingWithReflection;

WriteLine("Assembly metadata:");
Assembly? assembly = Assembly.GetEntryAssembly();

if (assembly is null)
{
    WriteLine("Failed to get entry assembly.");
    return;
}

WriteLine($" Full name: {assembly.FullName}");
WriteLine($" Location: {assembly.Location}");
WriteLine($" Entry point: {assembly.EntryPoint?.Name}");
IEnumerable<Attribute> attributes = assembly.GetCustomAttributes();
WriteLine($" Assembly-level attributes:");
foreach (Attribute a in attributes)
{
    WriteLine($" {a.GetType()}");
}

AssemblyInformationalVersionAttribute? version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
WriteLine($" Version: {version?.InformationalVersion}");

AssemblyCompanyAttribute? company = assembly.GetCustomAttribute<AssemblyCompanyAttribute>();
WriteLine($" Company: {company?.Company}"); // = WorkingWithReflection by defautl. Apps and Services with .NET -> see project definition


WriteLine();
WriteLine($"* Types:");
Type[] types = assembly.GetTypes();

foreach (Type type in types)
{
    WriteLine();
    WriteLine($"Type: {type.FullName}");
    MemberInfo[] members = type.GetMembers();

    foreach (MemberInfo member in members)
    {
        WriteLine("{0}: {1} ({2})", member.MemberType, member.Name, member.DeclaringType?.Name);
        IOrderedEnumerable<CoderAttribute> coders = member
            .GetCustomAttributes<CoderAttribute>()
            .OrderByDescending(c => c.LastModified);

        foreach (CoderAttribute coder in coders)
        {
            WriteLine("-> Modified by {0} on {1}", coder.Coder, coder.LastModified.ToShortDateString());
        }
    }
}