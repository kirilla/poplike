namespace Poplike.Application.Expressions.Commands.MoveExpressionUp;

public interface IMoveExpressionUpCommand
{
    Task Execute(IUserToken userToken, MoveExpressionUpCommandModel model);
}
