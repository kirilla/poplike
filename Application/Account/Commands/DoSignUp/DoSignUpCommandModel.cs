using Poplike.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.Account.Commands.DoSignUp;

public class DoSignUpCommandModel
{
    [RegularExpression(Pattern.Common.Email.Address)]
    [StringLength(MaxLengths.Common.Email.Address)]
    public string Email { get; set; }
}
