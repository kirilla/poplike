using NUnit.Framework;

namespace Poplike.Common.Enums;

[TestFixture]
public class EnumTests
{
    private void EnumValuesAreUnique<T>() where T : Enum
    {
        // Arrange, Act
        var collisions = Enum
            .GetValues(typeof(T))
            .Cast<int>()
            .ToList()
            .GroupBy(x => new
            {
                value = x,
            })
            .Where(x => x.Count() > 1)
            .ToList();

        // Assert
        Assert.That(collisions, Is.Empty, $"Value collision in enum {typeof(T).Name}.");
    }

    [Test]
    public void PageKind_EnumValuesAreUnique()
    {
        EnumValuesAreUnique<PageKind>();
    }
}
