using Poplike.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.Keywords.Commands.EditKeyword;

public class EditKeywordCommandModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Skriv ett nyckelord.")]
    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Keyword.Word,
        ErrorMessage = "Skriv kortare.")]
    public string Word { get; set; }
}
