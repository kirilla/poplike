using System.ComponentModel.DataAnnotations;
using Poplike.Common.Validation;

namespace Poplike.Application.Statements.Commands.AddUserStatement;

public class AddUserStatementCommandModel
{
    [Required]
    public int? SubjectId { get; set; }

    [RegularExpression(
        Pattern.Common.SomeContent,
        ErrorMessage = "Skriv något.")]
    [StringLength(
        MaxLengths.Domain.Statement.Sentence,
        ErrorMessage = "Skriv kortare.")]
    public string? Sentence { get; set; }
}
