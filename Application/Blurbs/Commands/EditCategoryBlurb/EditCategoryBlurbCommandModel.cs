using System.ComponentModel.DataAnnotations;
using Poplike.Common.Validation;

namespace Poplike.Application.Blurbs.Commands.EditCategoryBlurb;

public class EditCategoryBlurbCommandModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Skriv en text.")]
    [RegularExpression(Pattern.Common.AnythingMultiLine)]
    [StringLength(
        MaxLengths.Common.Blurb.Text,
        ErrorMessage = "Skriv kortare.")]
    public string Text { get; set; }
}
