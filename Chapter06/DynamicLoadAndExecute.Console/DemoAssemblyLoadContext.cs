using System.Runtime.Loader;

namespace DynamicLoadAndExecute.Console;

internal class DemoAssemblyLoadContext : AssemblyLoadContext
{
    private readonly AssemblyDependencyResolver _resolver;
    
    public DemoAssemblyLoadContext(string mainAssemblyToLoadPath)
        : base(isCollectible: true) =>
        _resolver = new AssemblyDependencyResolver(mainAssemblyToLoadPath);
}
