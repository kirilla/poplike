using Poplike.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.Expressions.Commands.AddExpression;

public class AddExpressionCommandModel
{
    [Required]
    public int? ExpressionSetId { get; set; }

    public string? GroupName { get; set; }
    public string? GroupEmoji { get; set; }

    [Required]
    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Expression.Characters,
        ErrorMessage = "Skriv kortare.")]
    public string Characters { get; set; }
}
