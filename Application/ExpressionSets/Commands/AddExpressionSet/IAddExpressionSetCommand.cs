namespace Poplike.Application.ExpressionSets.Commands.AddExpressionSet;

public interface IAddExpressionSetCommand
{
    Task<int> Execute(IUserToken userToken, AddExpressionSetCommandModel model);
}
