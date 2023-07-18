namespace Poplike.Application.Blurbs.Commands.RemoveSubjectBlurb;

public interface IRemoveSubjectBlurbCommand
{
    Task Execute(IUserToken userToken, RemoveSubjectBlurbCommandModel model);
}
