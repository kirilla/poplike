namespace Poplike.Application.Legal.Commands.EditRule;

public interface IEditRuleCommand
{
    Task Execute(IUserToken userToken, EditRuleCommandModel model);
}
