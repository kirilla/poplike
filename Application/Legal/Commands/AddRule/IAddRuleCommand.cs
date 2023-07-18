namespace Poplike.Application.Legal.Commands.AddRule;

public interface IAddRuleCommand
{
    Task<int> Execute(IUserToken userToken, AddRuleCommandModel model);
}
