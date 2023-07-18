using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.Subjects.Commands.ChangeSubjectExpressionSet;

public class ChangeSubjectExpressionSetCommandModel
{
    public int SubjectId { get; set; }

    [Required(ErrorMessage = "Välj något")]
    public int? ExpressionSetId { get; set; }

    public bool Confirmed { get; set; }
}
