namespace Poplike.Application.Statements.Commands.AddUserStatement;

public interface IAddUserStatementCommand
{
    Task Execute(IUserToken userToken, AddUserStatementCommandModel model);
}
