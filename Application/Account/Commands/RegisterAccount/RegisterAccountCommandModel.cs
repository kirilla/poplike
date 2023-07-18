using Poplike.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.Account.Commands.RegisterAccount;

public class RegisterAccountCommandModel
{
    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(MaxLengths.Domain.User.Name)]
    public string Name { get; set; }

    [RegularExpression(Pattern.Common.Email.Address)]
    [StringLength(MaxLengths.Common.Email.Address)]
    public string Email { get; set; }

    [StringLength(MaxLengths.Domain.User.Password)]
    public string Password { get; set; }

    public bool IsHidden { get; set; }
}
