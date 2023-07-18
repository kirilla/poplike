using Poplike.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.Localization.Commands.AddLanguage;

public class AddLanguageCommandModel
{
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
