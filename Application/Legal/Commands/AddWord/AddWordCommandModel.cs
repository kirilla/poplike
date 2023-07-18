using Poplike.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.Legal.Commands.AddWord;

public class AddWordCommandModel
{
    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Word.Value,
        ErrorMessage = "Försök att skriva kortare.")]
    public string Value { get; set; }
}
