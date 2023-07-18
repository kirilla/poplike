namespace Poplike.Domain;

public class Keyword : IEntity, ICreatedDateTime, IUpdatedDateTime
{
    public int Id { get; set; }

    public string Word { get; set; }

    public DateTime? Created { get; set; }
    public DateTime? Updated { get; set; }

    public int SubjectId { get; set; }
    public Subject Subject { get; set; }

    public EntityKind EntityKind => EntityKind.Keyword;
}
