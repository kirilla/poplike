namespace Poplike.Application.Subjects.Commands.EditSubject;

public interface IEditSubjectCommand
{
    Task Execute(IUserToken userToken, EditSubjectCommandModel model);
}
