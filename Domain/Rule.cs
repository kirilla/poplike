namespace Poplike.Domain;

public class Rule : IEntity, ICreatedDateTime, IUpdatedDateTime
{
    public int Id { get; set; }

    public int? Number { get; set; }

    public string Heading { get; set; }
    public string Text { get; set; }

    public DateTime? Created { get; set; }
    public DateTime? Updated { get; set; }

    public EntityKind EntityKind => EntityKind.Rule;
}
