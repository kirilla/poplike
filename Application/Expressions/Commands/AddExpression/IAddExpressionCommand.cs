namespace Poplike.Application.Expressions.Commands.AddExpression;

public interface IAddExpressionCommand
{
    Task<int> Execute(IUserToken userToken, AddExpressionCommandModel model);
}
