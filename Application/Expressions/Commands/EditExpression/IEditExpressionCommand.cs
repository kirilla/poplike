namespace Poplike.Application.Expressions.Commands.EditExpression;

public interface IEditExpressionCommand
{
    Task Execute(IUserToken userToken, EditExpressionCommandModel model);
}
