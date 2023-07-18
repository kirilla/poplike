namespace Poplike.Application.Statements.Commands.MoveStatementDown;

public interface IMoveStatementDownCommand
{
    Task Execute(IUserToken userToken, MoveStatementDownCommandModel model);
}
