using System.ComponentModel.DataAnnotations;
using Poplike.Common.Validation;

namespace Poplike.Application.Blurbs.Commands.AddCategoryBlurb;

public class AddCategoryBlurbCommandModel
{
    [Required]
    public int? CategoryId { get; set; }

    [Required(ErrorMessage = "Skriv en text.")]
    [RegularExpression(Pattern.Common.AnythingMultiLine)]
    [StringLength(
        MaxLengths.Common.Blurb.Text,
        ErrorMessage = "Skriv kortare.")]
    public string Text { get; set; }
}
