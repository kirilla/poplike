namespace Poplike.Domain;

public class SubjectContact :
    IEntity, ICreatedDateTime, IUpdatedDateTime, IFormatOnSave, IValidateOnSave
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? PhoneNumber { get; set; }
    public string? EmailAddress { get; set; }

    public string? Url { get; set; }

    public DateTime? Created { get; set; }
    public DateTime? Updated { get; set; }

    public int SubjectId { get; set; }
    public Subject Subject { get; set; }

    public EntityKind EntityKind => EntityKind.SubjectContact;

    public void FormatOnSave()
    {
        EmailAddress = EmailAddress?.Trim().ToLowerInvariant();
        PhoneNumber = PhoneNumber?.StripNonNumeric();

        this.SetEmptyStringsToNull();
    }

    public void ValidateOnSave()
    {
        if (!string.IsNullOrWhiteSpace(EmailAddress))
        {
            if (!RegexService.IsMatch(
                    EmailAddress, Pattern.Common.Email.Address))
                throw new ValidateOnSaveException();
        }
    }
}
