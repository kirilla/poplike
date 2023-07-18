namespace Poplike.Application.Statements.Commands.AddStatement;

public interface IAddStatementCommand
{
    Task Execute(IUserToken userToken, AddStatementCommandModel model);
}
