using System.Diagnostics;

namespace WorkingWithTasks
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            OutputThreadInfo();
            Stopwatch timer = Stopwatch.StartNew();

            SectionTitle("Running methods synchronously on one thread.");
            MethodA();
            MethodB();
            MethodC();

            WriteLine($"{timer.ElapsedMilliseconds:#,##0}ms elapsed.");

            #region Running methods asynchronously on multiple threads

            SectionTitle("Running methods asynchronously on multiple threads.");
            timer.Restart();

            //Three ways to start the methods using Task instances
            Task taskA = new(MethodA);
            taskA.Start();

            Task taskB = Task.Factory.StartNew(MethodB);

            Task taskC = Task.Run(MethodC);

            Task[] tasks = { taskA, taskB, taskC };
            Task.WaitAll(tasks);
            WriteLine($"{timer.ElapsedMilliseconds:#,##0}ms elapsed.");

            #endregion

            #region Continuation task

            SectionTitle("Passing the result of one task as an input into another.");
            timer.Restart();

            Task<string> taskServiceThenSProc = Task.Factory
                .StartNew(CallWebService) // returns Task<decimal>
                .ContinueWith(previousTask => // returns Task<string>
                    CallStoredProcedure(previousTask.Result));

            WriteLine($"Result: {taskServiceThenSProc.Result}");
            WriteLine($"{timer.ElapsedMilliseconds:#,##0}ms elapsed.");

            #endregion

            #region Nested and Child task

            SectionTitle("Nested and child tasks");
            Task outerTask = Task.Factory.StartNew(OuterMethod);
            outerTask.Wait();
            WriteLine("Console app is stopping.");

            #endregion
        }
    }
}