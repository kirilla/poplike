using System.ComponentModel.DataAnnotations;
using Poplike.Common.Validation;

namespace Poplike.Application.Legal.Commands.EditWord;

public class EditWordCommandModel
{
    public int Id { get; set; }

    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Word.Value,
        ErrorMessage = "Försök att skriva kortare.")]
    public string Value { get; set; }
}
