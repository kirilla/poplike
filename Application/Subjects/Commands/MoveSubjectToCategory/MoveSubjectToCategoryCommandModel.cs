using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.Subjects.Commands.MoveSubjectToCategory;

public class MoveSubjectToCategoryCommandModel
{
    public int Id { get; set; }

    [Required]
    public int? CategoryId { get; set; }
}
