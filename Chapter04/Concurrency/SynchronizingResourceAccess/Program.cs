using System.Diagnostics;

namespace SynchronizingResourceAccess;

internal partial class Program
{
    public static void Main(string[] args)
    {
        WriteLine("Please wait for the tasks to complete.");
        Stopwatch watch = Stopwatch.StartNew();

        //Task a = Task.Factory.StartNew(MethodALock);
        //Task b = Task.Factory.StartNew(MethodBLock);

        //Task a = Task.Factory.StartNew(MethodAMonitor);
        //Task b = Task.Factory.StartNew(MethodBLock);

        //WriteLine();
        //WriteLine($"Results: {SharedObjects.Message}.");
        //WriteLine($"{SharedObjects.Counter} string modifications.");

        BankAccount account = new();
        account.Deposit(100);
        

        WriteLine();
        WriteLine($"Balance: {account.Balance}.");

        WriteLine($"{watch.ElapsedMilliseconds:N0} elapsed milliseconds.");
    }
}