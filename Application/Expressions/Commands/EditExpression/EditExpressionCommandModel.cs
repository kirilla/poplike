using Poplike.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.Expressions.Commands.EditExpression;

public class EditExpressionCommandModel
{
    public int Id { get; set; }

    [Required]
    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Expression.Characters,
        ErrorMessage = "Skriv kortare.")]
    public string Characters { get; set; }
}
