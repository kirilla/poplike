namespace Poplike.Application.Expressions.Commands.RemoveExpression;

public interface IRemoveExpressionCommand
{
    Task Execute(IUserToken userToken, RemoveExpressionCommandModel model);
}
