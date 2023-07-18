namespace Poplike.Application.Categories.Commands.EditCategory;

public interface IEditCategoryCommand
{
    Task Execute(IUserToken userToken, EditCategoryCommandModel model);
}
