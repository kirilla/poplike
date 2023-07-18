namespace Poplike.Application.ExpressionSets.Commands.RemoveExpressionSet;

public interface IRemoveExpressionSetCommand
{
    Task Execute(IUserToken userToken, RemoveExpressionSetCommandModel model);
}
