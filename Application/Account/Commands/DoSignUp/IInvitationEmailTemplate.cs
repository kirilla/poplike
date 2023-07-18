namespace Poplike.Application.Account.Commands.DoSignUp
{
    public interface IInvitationEmailTemplate
    {
        Email Create(SignUp signup, Invitation invitation);
    }
}
