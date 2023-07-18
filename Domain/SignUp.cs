namespace Poplike.Domain;

public class SignUp :
    IEntity, ICreatedDateTime, IUpdatedDateTime, IFormatOnSave, IValidateOnSave
{
    public int Id { get; set; }

    public string EmailAddress { get; set; }

    public DateTime? Created { get; set; }
    public DateTime? Updated { get; set; }

    public List<Invitation> Invitations { get; set; }

    public EntityKind EntityKind => EntityKind.SignUp;

    public void FormatOnSave()
    {
        EmailAddress = EmailAddress.Trim().ToLowerInvariant();
    }

    public void ValidateOnSave()
    {
        if (string.IsNullOrWhiteSpace(EmailAddress))
            throw new ValidateOnSaveException();

        if (!RegexService.IsMatch(EmailAddress, Pattern.Common.Email.Address))
            throw new ValidateOnSaveException();
    }
}
