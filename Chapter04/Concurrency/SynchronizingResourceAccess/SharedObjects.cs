namespace SynchronizingResourceAccess;

internal static class SharedObjects
{
    public static string? Message; // a shared resource

    public static object Conch = new();

    // .NET events are not thread-safe, so you should avoid using them in multithreaded scenarios.
    // public event EventHandler? Shout;

    public static int Counter;
}
