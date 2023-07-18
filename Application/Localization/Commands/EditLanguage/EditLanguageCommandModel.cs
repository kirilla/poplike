using System.ComponentModel.DataAnnotations;
using Poplike.Common.Validation;

namespace Poplike.Application.Localization.Commands.EditLanguage;

public class EditLanguageCommandModel
{
    public int Id { get; set; }

    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Language.Name,
        ErrorMessage = "Försök att skriva kortare.")]
    public string Name { get; set; }

    [RegularExpression(Pattern.Common.Locale.Culture)]
    [StringLength(
        MaxLengths.Domain.Language.Culture,
        ErrorMessage = "Försök att skriva kortare.")]
    public string Culture { get; set; }

    [StringLength(
        MaxLengths.Domain.Language.Emoji,
        ErrorMessage = "Försök att skriva kortare.")]
    public string Emoji { get; set; }
}
