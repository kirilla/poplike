using System.ComponentModel.DataAnnotations;
using Poplike.Common.Validation;

namespace Poplike.Application.Contacts.Commands.AddCategoryContact;

public class AddCategoryContactCommandModel
{
    [Required]
    public int? CategoryId { get; set; }

    [Required(ErrorMessage = "Ge kontakten ett namn.")]
    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Common.Person.Name,
        ErrorMessage = "Skriv kortare.")]
    public string Name { get; set; }

    [RegularExpression(Pattern.Common.Phone.Number)]
    [StringLength(
        MaxLengths.Common.Phone.Number,
        ErrorMessage = "Skriv kortare.")]
    public string? PhoneNumber { get; set; }

    [RegularExpression(Pattern.Common.Email.Address)]
    [StringLength(
        MaxLengths.Common.Email.Address,
        ErrorMessage = "Skriv kortare.")]
    public string? EmailAddress { get; set; }

    [RegularExpression(
        Pattern.Common.Link.Url,
        ErrorMessage = "URL:en måste börja med http:// eller https://")]
    [StringLength(
        MaxLengths.Common.Link.Url,
        ErrorMessage = "Skriv kortare.")]
    public string Url { get; set; }
}
