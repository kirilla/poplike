namespace Poplike.Domain;

public class CategoryContact :
    IEntity,
    ICreatedDateTime, IUpdatedDateTime,
    IFormatOnSave, IValidateOnSave
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? PhoneNumber { get; set; }
    public string? EmailAddress { get; set; }

    public string? Url { get; set; }

    public DateTime? Created { get; set; }
    public DateTime? Updated { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }

    public EntityKind EntityKind => EntityKind.CategoryContact;

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
