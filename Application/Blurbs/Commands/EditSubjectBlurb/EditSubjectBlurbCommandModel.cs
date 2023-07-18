using Poplike.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.Blurbs.Commands.EditSubjectBlurb;

public class EditSubjectBlurbCommandModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Skriv en text.")]
    [RegularExpression(Pattern.Common.AnythingMultiLine)]
    [StringLength(
        MaxLengths.Common.Blurb.Text,
        ErrorMessage = "Skriv kortare.")]
    public string Text { get; set; }
}
