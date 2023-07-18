namespace Poplike.Application.Statements.Commands.DeleteUserStatement;

public interface IDeleteUserStatementCommand
{
    Task Execute(IUserToken userToken, DeleteUserStatementCommandModel model);
}
