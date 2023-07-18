namespace Poplike.Domain;

public class CategoryBlurb : IEntity, ICreatedDateTime, IUpdatedDateTime
{
    public int Id { get; set; }

    public string Text { get; set; }

    public DateTime? Created { get; set; }
    public DateTime? Updated { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public EntityKind EntityKind => EntityKind.CategoryBlurb;
}
