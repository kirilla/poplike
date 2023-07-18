namespace Poplike.Application.Categories.Commands.RemoveCategory;

public interface IRemoveCategoryCommand
{
    Task Execute(IUserToken userToken, RemoveCategoryCommandModel model);
}
