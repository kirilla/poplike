namespace Poplike.Application.Subjects.Commands.DeleteSubject;

public interface IDeleteSubjectCommand
{
    Task Execute(IUserToken userToken, DeleteSubjectCommandModel model);
}
