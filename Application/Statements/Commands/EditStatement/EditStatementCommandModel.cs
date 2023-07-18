using Poplike.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.Statements.Commands.EditStatement;

public class EditStatementCommandModel
{
    public int Id { get; set; }

    [Required]
    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Statement.Sentence,
        ErrorMessage = "Skriv kortare.")]
    public string Sentence { get; set; }
}
