namespace Poplike.Application.Legal.Commands.RemoveRule;

public interface IRemoveRuleCommand
{
    Task Execute(IUserToken userToken, RemoveRuleCommandModel model);
}
