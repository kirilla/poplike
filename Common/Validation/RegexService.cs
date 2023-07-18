using System.Text.RegularExpressions;

namespace Poplike.Common.Validation;

public static class RegexService
{
    public static bool IsMatch(string input, string pattern)
    {
        return Regex.IsMatch(
            input, pattern,
            RegexOptions.None,
            TimeSpan.FromMilliseconds(300));
    }
}
