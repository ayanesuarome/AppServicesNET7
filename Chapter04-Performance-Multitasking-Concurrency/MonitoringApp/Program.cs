// See https://aka.ms/new-console-template for more information
using MonitoringLib;
using System.Text;

namespace MonitoringApp;

public partial class Program
{
    public static void Main()
    {
        WriteLine("Processing. Please wait...");
        /*
        Recorder.Start();

        // simulate a process that requires some memory resources...
        int[] largeArrayOfInts = Enumerable.Range(start: 1, count: 10_000).ToArray();

        // ...and takes some time to complete
        Thread.Sleep(new Random().Next(5, 10) * 1000);
        Recorder.Stop();
        */

        int[] numbers = Enumerable.Range(start: 1, count: 50_000).ToArray();

        #region Using StringBuilder
        SectionTitle("Using StringBuilder");
        Recorder.Start();
        StringBuilder builder = new();

        for (int i = 0; i < numbers.Length; i++)
        {
            builder.Append(numbers[i]);
            builder.Append(", ");
        }

        Recorder.Stop();
        #endregion
        
        WriteLine();

        #region Using string with +
        SectionTitle("Using string with +");
        Recorder.Start();

        string s = string.Empty; // i.e. ""

        for (int i = 0; i < numbers.Length; i++)
        {
            s += numbers[i] + ", ";
        }

        Recorder.Stop();
        #endregion
    }
}
