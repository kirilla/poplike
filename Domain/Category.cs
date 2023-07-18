namespace Poplike.Domain;

public class Category : IEntity
{
    public int Id { get; set; }

    public string Emoji { get; set; }
    public string Name { get; set; }

    public string SubjectHeading { get; set; }
    public string SubjectPlaceholder { get; set; }

    public int ExpressionSetId { get; set; }
    public ExpressionSet ExpressionSet { get; set; }

    public List<CategoryContact> CategoryContacts { get; set; }
    public List<CategoryBlurb> CategoryBlurbs { get; set; }
    public List<Subject> Subjects { get; set; }

    public EntityKind EntityKind => EntityKind.Category;
}
