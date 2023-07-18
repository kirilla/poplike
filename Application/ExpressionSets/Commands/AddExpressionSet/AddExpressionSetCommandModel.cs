using Poplike.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.ExpressionSets.Commands.AddExpressionSet;

public class AddExpressionSetCommandModel
{
    [Required(ErrorMessage = "Lägg till en sammanfattande symbol.")]
    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.ExpressionSet.Emoji,
        ErrorMessage = "Skriv kortare.")]
    public string Emoji { get; set; }

    [Required(ErrorMessage = "Lägg till en sammanfattande textbeskrivning.")]
    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.ExpressionSet.Name,
        ErrorMessage = "Skriv kortare.")]
    public string Name { get; set; }

    public bool MultipleChoice { get; set; }
    public bool FreeExpression { get; set; }

    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Expression.Characters,
        ErrorMessage = "Skriv kortare.")]
    public string? Expression1 { get; set; }

    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Expression.Characters,
        ErrorMessage = "Skriv kortare.")]
    public string? Expression2 { get; set; }

    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Expression.Characters,
        ErrorMessage = "Skriv kortare.")]
    public string? Expression3 { get; set; }

    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Expression.Characters,
        ErrorMessage = "Skriv kortare.")]
    public string? Expression4 { get; set; }

    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Expression.Characters,
        ErrorMessage = "Skriv kortare.")]
    public string? Expression5 { get; set; }
}
