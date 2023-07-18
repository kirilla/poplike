namespace Poplike.Application.Account.Commands.RequestPasswordReset;

public interface IPasswordResetEmailTemplate
{
    Email Create(User user, PasswordResetRequest resetRequest);
}
