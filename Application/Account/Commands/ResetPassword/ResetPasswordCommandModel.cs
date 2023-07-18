using System.ComponentModel.DataAnnotations;
using Poplike.Common.Validation;

namespace Poplike.Application.Account.Commands.ResetPassword;

public class ResetPasswordCommandModel
{
    public Guid? Guid { get; set; }

    [RegularExpression(Pattern.Common.Email.Address)]
    [StringLength(MaxLengths.Common.Email.Address)]
    public string Email { get; set; }

    [StringLength(MaxLengths.Domain.User.Password)]
    public string Password { get; set; }
}
