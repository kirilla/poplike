using Poplike.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.Keywords.Commands.AddKeyword;

public class AddKeywordCommandModel
{
    [Required]
    public int? SubjectId { get; set; }

    [Required(ErrorMessage = "Skriv ett nyckelord.")]
    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Keyword.Word,
        ErrorMessage = "Skriv kortare.")]
    public string Word { get; set; }
}
