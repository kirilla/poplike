namespace Poplike.Application.Blurbs.Commands.EditCategoryBlurb;

public interface IEditCategoryBlurbCommand
{
    Task Execute(IUserToken userToken, EditCategoryBlurbCommandModel model);
}
