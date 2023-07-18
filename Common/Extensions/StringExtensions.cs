using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Poplike.Common.Extensions;

public static class StringExtensions
{
    public static string MakeUrlFriendly(this string s)
    {
        s = s.Trim();
        s = s.RemoveDiacritics();
        s = s.ToLower();

        s = Regex.Replace(s, @"[^a-z0-9\-\s]", "");
        s = Regex.Replace(s, @"\s", "-");
        s = Regex.Replace(s, @"-{2,}", "-");

        s = s.Trim('-');

        return s;
    }

    public static string RemoveDiacritics(this string s)
    {
        // https://stackoverflow.com/a/72269490
        // CC BY-SA 4.0
        // Dmitry Bychenko, Michael S. Kaplan

        return string.Concat(s
            .Normalize(NormalizationForm.FormD)
            .Where(x =>
                CharUnicodeInfo.GetUnicodeCategory(x)
                    != UnicodeCategory.NonSpacingMark))
            .Normalize(NormalizationForm.FormC);
    }

    public static List<string> SplitCategoryPath(this string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            return new List<string>();

        return s
            .Split('/', 
                StringSplitOptions.TrimEntries | 
                StringSplitOptions.RemoveEmptyEntries)
            .ToList();
    }

    public static List<string> SplitOnDelimiters(this string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            return new List<string>();

        var delimiters = new string[] { "\r\n", "\r", "\n", "\f", "\t", "\v" };

        return s
            .Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
            .ToList();
    }

    public static string StripNonNumeric(this string value)
    {
        var regex = new Regex(
            @"\D", 
            RegexOptions.CultureInvariant, 
            TimeSpan.FromSeconds(1));

        return regex.Replace(value, "");
    }

    public static string? Truncate(this string? value, int maxLength)
    {
        if (value == null)
            return null;

        if (value.Length <= maxLength)
            return value;

        return value.Substring(0, maxLength);
    }

    public static string? TruncateWithSuffix(this string? value, int maxLength, string truncationSuffix = "…")
    {
        if (value == null)
            return null;

        if (value.Length <= maxLength)
            return value;

        return 
            value.Substring(0, maxLength - truncationSuffix.Length) +
                truncationSuffix;
    }
}
