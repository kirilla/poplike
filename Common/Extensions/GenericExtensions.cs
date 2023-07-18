namespace Poplike.Common.Extensions;

public static class GenericExtensions
{
    public static void SetEmptyStringsToNull<T>(this T obj)
    {
        var properties = obj!
            .GetType()
            .GetProperties()
            .Where(p =>
                p.PropertyType == typeof(string) &&
                p.GetGetMethod() != null &&
                p.GetSetMethod() != null);

        foreach (var property in properties)
        {
            string? value = (string?)property.GetValue(obj, null);

            if (value != null && string.IsNullOrWhiteSpace(value))
                property.SetValue(obj, null, null);
        }
    }

    public static void SetNullStringsToEmpty<T>(this T obj)
    {
        var properties = obj!
            .GetType()
            .GetProperties()
            .Where(p =>
                p.PropertyType == typeof(string) &&
                p.GetGetMethod() != null &&
                p.GetSetMethod() != null);

        foreach (var property in properties)
        {
            string? value = (string?)property.GetValue(obj, null);

            if (value == null)
                property.SetValue(obj, string.Empty, null);
        }
    }

    public static void TrimStringProperties<T>(this T obj)
    {
        var properties = obj!
            .GetType()
            .GetProperties()
            .Where(p =>
                p.PropertyType == typeof(string) &&
                p.GetGetMethod() != null &&
                p.GetSetMethod() != null);

        foreach (var property in properties)
        {
            string? value = (string?)property.GetValue(obj, null);

            if (value != null)
                property.SetValue(obj, value.Trim(), null);
        }
    }
}
