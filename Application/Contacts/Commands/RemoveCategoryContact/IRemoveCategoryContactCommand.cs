namespace Poplike.Application.Contacts.Commands.RemoveCategoryContact;

public interface IRemoveCategoryContactCommand
{
    Task Execute(IUserToken userToken, RemoveCategoryContactCommandModel model);
}
