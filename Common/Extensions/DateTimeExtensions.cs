using System.Globalization;

namespace Poplike.Common.Extensions;

public static class DateTimeExtensions
{
    public static string? ToFixedFormatDate(this DateTime date)
    {
        return date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
    }

    public static string? ToFixedFormatDate(this DateTime? date)
    {
        return date?.ToFixedFormatDate();
    }

    public static string ToFixedFormatDateShortTime(this DateTime date)
    {
        return date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
    }

    public static string? ToFixedFormatDateShortTime(this DateTime? date)
    {
        return date?.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
    }

    public static DateTime? TryParseDate(this string? s)
    {
        if (string.IsNullOrWhiteSpace(s))
            return null;

        var success = DateTime.TryParse(s, out DateTime date);

        if (success == false)
            return null;

        return date;
    }
}
