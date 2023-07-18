using Poplike.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.ExpressionSets.Commands.EditExpressionSet;

public class EditExpressionSetCommandModel
{
    public int Id { get; set; }

    [Required]
    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.ExpressionSet.Emoji,
        ErrorMessage = "Skriv kortare.")]
    public string Emoji { get; set; }

    [Required]
    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.ExpressionSet.Name,
        ErrorMessage = "Skriv kortare.")]
    public string Name { get; set; }

    public bool MultipleChoice { get; set; }
    public bool FreeExpression { get; set; }
}
