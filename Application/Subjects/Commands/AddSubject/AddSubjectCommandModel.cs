using Poplike.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.Subjects.Commands.AddSubject;

public class AddSubjectCommandModel
{
    [Required]
    public int? CategoryId { get; set; }

    [Required(ErrorMessage = "Namn måste anges.")]
    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Subject.Name,
        ErrorMessage = "Skriv kortare.")]
    public string Name { get; set; }

    [Required(ErrorMessage =
        "Välj en uppsättning svarsalternativ")]
    public int? ExpressionSetId { get; set; }

    public string? GroupName { get; set; }
}
