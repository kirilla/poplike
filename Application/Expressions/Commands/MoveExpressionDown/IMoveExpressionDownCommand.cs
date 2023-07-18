namespace Poplike.Application.Expressions.Commands.MoveExpressionDown;

public interface IMoveExpressionDownCommand
{
    Task Execute(IUserToken userToken, MoveExpressionDownCommandModel model);
}
