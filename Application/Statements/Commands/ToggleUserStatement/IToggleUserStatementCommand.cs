namespace Poplike.Application.Statements.Commands.ToggleUserStatement;

public interface IToggleUserStatementCommand
{
    Task Execute(IUserToken userToken, ToggleUserStatementCommandModel model);
}
