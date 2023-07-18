namespace Poplike.Application.Subjects.Commands.AddSubject;

public interface IAddSubjectCommand
{
    Task<int> Execute(IUserToken userToken, AddSubjectCommandModel model);
}
