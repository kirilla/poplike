namespace Poplike.Application.Statements.Commands.RemoveStatement;

public interface IRemoveStatementCommand
{
    Task Execute(IUserToken userToken, RemoveStatementCommandModel model);
}
