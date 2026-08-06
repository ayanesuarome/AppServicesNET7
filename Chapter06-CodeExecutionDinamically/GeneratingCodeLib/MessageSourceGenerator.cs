using Microsoft.CodeAnalysis;

namespace GeneratingCodeLib;

[Generator]
public class MessageSourceGenerator : ISourceGenerator
{
    public void Execute(GeneratorExecutionContext context)
    {
        IMethodSymbol mainMethod = context.Compilation
            .GetEntryPoint(context.CancellationToken);

        string typeName = mainMethod.ContainingType.Name;
        string sourceCode =
            $@"
            public static partial class {typeName}
            {{
                static partial void Message(string message)
                {{
                    System.Console.WriteLine($""Generator says: '{{message}}'"");
                }}
            }}";

        // Good Practice: Include .g. or .generated. in the filename of source-generated files.
        context.AddSource($"{typeName}.Methods.g.cs", sourceCode);
    }

    public void Initialize(GeneratorInitializationContext context)
    {
    }
}