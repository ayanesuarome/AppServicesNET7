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
        WriteLine($"Christmas is day {xmas.DayOfYear} of the year 2025.");
        WriteLine($"Christmas {xmas.Year} is on a {xmas.DayOfWeek}.");

        WriteLine("");

        SectionTitle("Date and time calculations");

        DateTime beforeXmas = xmas.Subtract(TimeSpan.FromDays(12));
        DateTime afterXmas = xmas.AddDays(12);
        // :d means format as short date only without time
        WriteLine($"12 days before Christmas: {beforeXmas:d}");
        WriteLine($"12 days after Christmas: {afterXmas:d}");
        TimeSpan untilXmas = xmas - DateTime.Now;
        WriteLine($"Now: {DateTime.Now}");
        WriteLine(
            "There are {0} days and {1} hours until Christmas 2025.",
            arg0: untilXmas.Days,
            arg1: untilXmas.Hours);
        WriteLine("There are {0:N0} hours until Christmas 2025.",
        arg0: untilXmas.TotalHours);

        WriteLine("");

        DateTime kidsWakeUp = new(
            year: 2025,
            month: 12,
            day: 25,
            hour: 6,
            minute: 30,
            second: 0);

        WriteLine($"Kids wake up: {kidsWakeUp}");
        WriteLine("The kids woke me up at {0}", arg0: kidsWakeUp.ToShortTimeString());

        WriteLine("");

        // A tick is 100 nanoseconds
        SectionTitle("Milli-, micro-, and nanoseconds");
        DateTime preciseTime = new(
            year: 2022, 
            month: 11,
            day: 8,
            hour: 12,
            minute: 0,
            second: 0,
            millisecond: 6,
            microsecond: 999);
        WriteLine("Millisecond: {0}, Microsecond: {1}, Nanosecond: {2}",
            preciseTime.Millisecond,
            preciseTime.Microsecond,
            preciseTime.Nanosecond);
        preciseTime = DateTime.UtcNow;
        // Nanosecond value will be 0 to 900 in 100 nanosecond increments.
        WriteLine("Millisecond: {0}, Microsecond: {1}, Nanosecond: {2}",
            preciseTime.Millisecond,
            preciseTime.Microsecond,
            preciseTime.Nanosecond);

        WriteLine("");

        SectionTitle("Globalization with dates and times");
        // same as Thread.CurrentThread.CurrentCulture
        WriteLine($"Current culture is: {CultureInfo.CurrentCulture.Name}");
        string textDate = "4 July 2025";
        DateTime independenceDay = DateTime.Parse(textDate);
        WriteLine($"Text: {textDate}, DateTime: {independenceDay:d MMMM}");
        textDate = "7/4/2025";
        independenceDay = DateTime.Parse(textDate);
        WriteLine($"Text: {textDate}, DateTime: {independenceDay:d MMMM}");
        independenceDay = DateTime.Parse(textDate, provider: CultureInfo.GetCultureInfo("en-US"));
        WriteLine($"Text: {textDate}, DateTime (en-US): {independenceDay:d MMMM}");

        WriteLine("");

        for (int year = 2022; year <= 2028; year++)
        {
            Write($"{year} is a leap year: {DateTime.IsLeapYear(year)}. ");
            WriteLine(
                "There are {0} days in February {1}.",
                arg0: DateTime.DaysInMonth(year: year, month: 2),
                arg1: year);
        }
        WriteLine("Is Christmas daylight saving time? {0}", arg0: xmas.IsDaylightSavingTime());
        WriteLine("Is July 4th daylight saving time? {0}", arg0: independenceDay.IsDaylightSavingTime());
        WriteLine("Is Octuber 26th daylight saving time? {0}", arg0: DateTime.Parse("26 October 2025").IsDaylightSavingTime());
        WriteLine("Is today daylight saving time? {0}", arg0: DateTime.Now.IsDaylightSavingTime());
    }
}
