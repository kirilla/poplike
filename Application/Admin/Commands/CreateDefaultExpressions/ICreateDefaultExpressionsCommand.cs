namespace Poplike.Application.Admin.Commands.CreateDefaultExpressions;

public interface ICreateDefaultExpressionsCommand
{
    Task Execute(IUserToken userToken, CreateDefaultExpressionsCommandModel model);
}
