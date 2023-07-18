namespace Poplike.Application.ExpressionSets.Commands.EditExpressionSet;

public interface IEditExpressionSetCommand
{
    Task Execute(IUserToken userToken, EditExpressionSetCommandModel model);
}
