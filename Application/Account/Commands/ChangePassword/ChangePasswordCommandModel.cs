using System.ComponentModel.DataAnnotations;
using Poplike.Common.Validation;

namespace Poplike.Application.Account.Commands.ChangePassword;

public class ChangePasswordCommandModel
{
    [StringLength(MaxLengths.Domain.User.Password)]
    public string OldPassword { get; set; }

    [StringLength(MaxLengths.Domain.User.Password)]
    public string NewPassword { get; set; }
}
