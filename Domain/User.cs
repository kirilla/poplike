namespace Poplike.Domain;

public class User :
    IEntity, ICreatedDateTime, IUpdatedDateTime, IFormatOnSave, IValidateOnSave
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string EmailAddress { get; set; }
    public string PhoneNumber { get; set; }

    public string PasswordHash { get; set; }

    public Guid? Guid { get; set; }

    public bool IsAdmin { get; set; }
    public bool IsCurator { get; set; }
    public bool IsModerator { get; set; }

    public DateTime? Created { get; set; }
    public DateTime? Updated { get; set; }

    public bool IsHidden { get; set; }

    public List<PasswordResetRequest> PasswordResetRequests { get; set; }
    public List<UserStatement> UserStatements { get; set; }
    public List<Session> Sessions { get; set; }

    public EntityKind EntityKind => EntityKind.User;

    public void FormatOnSave()
    {
        EmailAddress = EmailAddress.Trim().ToLowerInvariant();
        PhoneNumber = PhoneNumber.StripNonNumeric();
    }

    public void ValidateOnSave()
    {
        if (string.IsNullOrWhiteSpace(EmailAddress))
            throw new ValidateOnSaveException();

        if (!RegexService.IsMatch(EmailAddress, Pattern.Common.Email.Address))
            throw new ValidateOnSaveException();
    }
}
