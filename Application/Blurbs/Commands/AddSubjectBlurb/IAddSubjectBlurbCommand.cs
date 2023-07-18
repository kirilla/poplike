namespace Poplike.Application.Blurbs.Commands.AddSubjectBlurb;

public interface IAddSubjectBlurbCommand
{
    Task<int> Execute(IUserToken userToken, AddSubjectBlurbCommandModel model);
}
