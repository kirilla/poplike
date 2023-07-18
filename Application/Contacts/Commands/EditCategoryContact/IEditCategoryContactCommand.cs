namespace Poplike.Application.Contacts.Commands.EditCategoryContact;

public interface IEditCategoryContactCommand
{
    Task Execute(IUserToken userToken, EditCategoryContactCommandModel model);
}
