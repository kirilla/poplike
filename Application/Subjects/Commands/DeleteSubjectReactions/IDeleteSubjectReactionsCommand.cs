namespace Poplike.Application.Subjects.Commands.DeleteSubjectReactions;

public interface IDeleteSubjectReactionsCommand
{
    Task Execute(IUserToken userToken, DeleteSubjectReactionsCommandModel model);
}
