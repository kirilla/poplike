using Poplike.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.Subjects.Commands.EditSubject;

public class EditSubjectCommandModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Saken måste ha ett namn.")]
    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(
        MaxLengths.Domain.Subject.Name,
        ErrorMessage = "Skriv kortare.")]
    public string Name { get; set; }

    public bool MultipleChoice { get; set; }
    public bool FreeExpression { get; set; }
}
