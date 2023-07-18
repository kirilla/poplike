namespace Poplike.Application.Blurbs.Commands.RemoveCategoryBlurb;

public interface IRemoveCategoryBlurbCommand
{
    Task Execute(IUserToken userToken, RemoveCategoryBlurbCommandModel model);
}
