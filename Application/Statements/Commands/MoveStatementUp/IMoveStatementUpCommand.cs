namespace Poplike.Application.Statements.Commands.MoveStatementUp;

public interface IMoveStatementUpCommand
{
    Task Execute(IUserToken userToken, MoveStatementUpCommandModel model);
}
