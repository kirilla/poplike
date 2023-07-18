using Poplike.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.Users.Commands.EditUser;

public class EditUserCommandModel
{
    public int Id { get; set; }

    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(MaxLengths.Domain.User.Name)]
    public string Name { get; set; }
}
