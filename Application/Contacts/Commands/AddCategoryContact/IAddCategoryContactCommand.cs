namespace Poplike.Application.Contacts.Commands.AddCategoryContact;

public interface IAddCategoryContactCommand
{
    Task<int> Execute(IUserToken userToken, AddCategoryContactCommandModel model);
}
