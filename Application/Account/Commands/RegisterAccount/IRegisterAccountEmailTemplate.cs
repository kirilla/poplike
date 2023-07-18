namespace Poplike.Application.Account.Commands.RegisterAccount
{
    public interface IRegisterAccountEmailTemplate
    {
        Email Create(User user);
    }
}
