namespace Poplike.Domain;

public class SubjectBlurb : IEntity, ICreatedDateTime, IUpdatedDateTime
{
    public int Id { get; set; }

    public string Text { get; set; }

    public DateTime? Created { get; set; }
    public DateTime? Updated { get; set; }

    public int SubjectId { get; set; }
    public Subject Subject { get; set; }

    public EntityKind EntityKind => EntityKind.SubjectBlurb;
}
