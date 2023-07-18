namespace Poplike.Domain;

public class Invitation : IEntity, ICreatedDateTime, IUpdatedDateTime
{
    public int Id { get; set; }

    public Guid? Guid { get; set; }

    public DateTime? Created { get; set; }
    public DateTime? Updated { get; set; }

    public int SignUpId { get; set; }
    public SignUp SignUp { get; set; }

    public EntityKind EntityKind => EntityKind.Invitation;
}
