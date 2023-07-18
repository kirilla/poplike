namespace Poplike.Application.Blurbs.Commands.AddCategoryBlurb;

public interface IAddCategoryBlurbCommand
{
    Task<int> Execute(IUserToken userToken, AddCategoryBlurbCommandModel model);
}
