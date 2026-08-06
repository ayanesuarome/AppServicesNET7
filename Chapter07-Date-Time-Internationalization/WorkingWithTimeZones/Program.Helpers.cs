using System.Collections.ObjectModel;

namespace WorkingWithTimeZones;

internal partial class Program
{
    static void SectionTitle(string title)
    {
        ConsoleColor previousColor = ForegroundColor;
        ForegroundColor = ConsoleColor.DarkYellow;
        WriteLine("*");
        WriteLine($"* {title}");
        WriteLine("*");
        ForegroundColor = previousColor;
    }

    static void OutputTimeZones()
    {
        // get the time zones registered with the OS
        ReadOnlyCollection<TimeZoneInfo> zones = TimeZoneInfo.GetSystemTimeZones();
        WriteLine("*");
        WriteLine($"* {zones.Count} time zones:");
        WriteLine("*");

        // order the time zones by Id instead of DisplayName
        foreach (TimeZoneInfo zone in zones.OrderBy(z => z.Id))
        {
            WriteLine($"{zone.Id}");
        }
    }

    static void OutputDateTime(DateTime dateTime, string title)
    {
        SectionTitle(title);
        WriteLine($"Value: {dateTime}");
        WriteLine($"Kind: {dateTime.Kind}");
        WriteLine($"IsDaylightSavingTime: {dateTime.IsDaylightSavingTime()}");
        WriteLine($"ToLocalTime(): {dateTime.ToLocalTime()}");
        WriteLine($"ToUniversalTime(): {dateTime.ToUniversalTime()}");
    }
    static void OutputTimeZone(TimeZoneInfo zone, string title)
    {
        SectionTitle(title);
        WriteLine($"Id: {zone.Id}");
        WriteLine("IsDaylightSavingTime(DateTime.Now): {0}",
        zone.IsDaylightSavingTime(DateTime.Now));
        WriteLine($"StandardName: {zone.StandardName}");
        WriteLine($"DaylightName: {zone.DaylightName}");
        WriteLine($"BaseUtcOffset: {zone.BaseUtcOffset}");
    }

    /// <summary>
    /// Time zone names change if Daylight Saving time is active.
    /// e.g. GMT Standard Time becomes GMT Summer Time.
    /// </summary>
    static string GetCurrentZoneName(TimeZoneInfo zone, DateTime when) =>
        zone.IsDaylightSavingTime(when) ?
            zone.DaylightName :
            zone.StandardName;
}
