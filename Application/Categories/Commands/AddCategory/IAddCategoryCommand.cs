namespace Poplike.Application.Categories.Commands.AddCategory;

public interface IAddCategoryCommand
{
    Task<int> Execute(IUserToken userToken, AddCategoryCommandModel model);
}
