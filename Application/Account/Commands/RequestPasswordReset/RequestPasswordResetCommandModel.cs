using System.ComponentModel.DataAnnotations;
using Poplike.Common.Validation;

namespace Poplike.Application.Account.Commands.RequestPasswordReset;

public class RequestPasswordResetCommandModel
{
    [RegularExpression(Pattern.Common.Email.Address)]
    [StringLength(MaxLengths.Common.Email.Address)]
    public string Email { get; set; }
}
