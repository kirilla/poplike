namespace Poplike.Application.Subjects.Commands.ChangeSubjectExpressionSet;

public interface IChangeSubjectExpressionSetCommand
{
    Task Execute(IUserToken userToken, ChangeSubjectExpressionSetCommandModel model);
}
