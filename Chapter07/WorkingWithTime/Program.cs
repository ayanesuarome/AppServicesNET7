using System.Globalization;

namespace WorkingWithTime;

internal partial class Program
{
    static void Main(string[] args)
    {
        Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("en-GB");

        SectionTitle("Specifying date and time values");

        WriteLine($"DateTime.MinValue: {DateTime.MinValue}");
        WriteLine($"DateTime.MaxValue: {DateTime.MaxValue}");
        WriteLine($"DateTime.UnixEpoch: {DateTime.UnixEpoch}");
        WriteLine($"DateTime.Now: {DateTime.Now}");
        WriteLine($"DateTime.Today: {DateTime.Today}");

        WriteLine("");

        DateTime xmas = new(year: 2025, month: 12, day: 25);

        WriteLine($"Christmas (default format): {xmas}");
        WriteLine($"Christmas (custom format): {xmas:dddd, dd MMMM yyyy}");
        WriteLine($"Christmas is in month {xmas.Month} of the year.");
        WriteLine($"Christmas is day {xmas.DayOfYear} of the year 2024.");
        WriteLine($"Christmas {xmas.Year} is on a {xmas.DayOfWeek}.");
    }
}
