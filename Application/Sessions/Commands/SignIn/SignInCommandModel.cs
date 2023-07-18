using System.ComponentModel.DataAnnotations;
using Poplike.Common.Validation;

namespace Poplike.Application.Sessions.Commands.SignIn;

public class SignInCommandModel
{
    [RegularExpression(
        Pattern.Common.Email.Address, 
        ErrorMessage = "Oväntat format på epostadressen.")]
    [StringLength(MaxLengths.Common.Email.Address)]
    public string Email { get; set; }

    [StringLength(MaxLengths.Domain.User.Password)]
    public string Password { get; set; }
}
