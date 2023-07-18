using Poplike.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.Categories.Commands.AddCategory;

public class AddCategoryCommandModel
{
    [Required(ErrorMessage = "Välj en emoji eller symbol.")]
    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Category.Emoji,
        ErrorMessage = "Skriv kortare.")]
    public string Emoji { get; set; }

    [Required(ErrorMessage = "Ge kategorin ett namn.")]
    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Category.Name,
        ErrorMessage = "Skriv kortare.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Skriv en rubrik för 'lägg till'-dialogen.")]
    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Category.SubjectHeading,
        ErrorMessage = "Skriv kortare.")]
    public string SubjectHeading { get; set; }

    [Required(ErrorMessage = 
        "Skriv en hjälptext för inmatningsfältet i 'lägg till'-dialogen.")]
    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Category.SubjectPlaceholder,
        ErrorMessage = "Skriv kortare.")]
    public string SubjectPlaceholder { get; set; }

    [Required(ErrorMessage = 
        "Välj en uppsättning svarsalternativ")]
    public int? ExpressionSetId { get; set; }
}
