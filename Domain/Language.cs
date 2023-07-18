namespace Poplike.Domain;

public class Language : IEntity, ICreatedDateTime, IUpdatedDateTime, IFormatOnSave
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Culture { get; set; }
    public string Emoji { get; set; }

    public DateTime? Created { get; set; }
    public DateTime? Updated { get; set; }

    public EntityKind EntityKind => EntityKind.Language;

    public void FormatOnSave()
    {
        Culture = Culture?.Trim().ToLowerInvariant();
    }
}
