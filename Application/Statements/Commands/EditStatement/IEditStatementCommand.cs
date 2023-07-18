namespace Poplike.Application.Statements.Commands.EditStatement;

public interface IEditStatementCommand
{
    Task Execute(IUserToken userToken, EditStatementCommandModel model);
}
