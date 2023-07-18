using Poplike.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.Blurbs.Commands.AddSubjectBlurb;

public class AddSubjectBlurbCommandModel
{
    [Required]
    public int? SubjectId { get; set; }

    [Required(ErrorMessage = "Skriv en text.")]
    [RegularExpression(Pattern.Common.AnythingMultiLine)]
    [StringLength(
        MaxLengths.Common.Blurb.Text,
        ErrorMessage = "Skriv kortare.")]
    public string Text { get; set; }
}
