namespace Poplike.Domain;

public class Word : IEntity, ICreatedDateTime, IUpdatedDateTime
{
    public int Id { get; set; }

    public string Value { get; set; }

    public DateTime? Created { get; set; }
    public DateTime? Updated { get; set; }

    public EntityKind EntityKind => EntityKind.Word;
}
