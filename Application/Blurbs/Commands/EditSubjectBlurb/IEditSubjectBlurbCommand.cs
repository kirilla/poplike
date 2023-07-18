namespace Poplike.Application.Blurbs.Commands.EditSubjectBlurb;

public interface IEditSubjectBlurbCommand
{
    Task Execute(IUserToken userToken, EditSubjectBlurbCommandModel model);
}
