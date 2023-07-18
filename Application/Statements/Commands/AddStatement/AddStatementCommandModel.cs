using Poplike.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.Statements.Commands.AddStatement;

public class AddStatementCommandModel
{
    [Required]
    public int? SubjectId { get; set; }

    [Required]
    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Statement.Sentence,
        ErrorMessage = "Skriv kortare.")]
    public string Sentence { get; set; }
}
