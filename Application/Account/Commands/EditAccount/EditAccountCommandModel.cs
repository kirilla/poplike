using Poplike.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.Account.Commands.EditAccount;

public class EditAccountCommandModel
{
    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(MaxLengths.Domain.User.Name)]
    public string Name { get; set; }

    [RegularExpression(Pattern.Common.Email.Address)]
    [StringLength(MaxLengths.Common.Email.Address)]
    public string Email { get; set; }

    public bool IsHidden { get; set; }
}
