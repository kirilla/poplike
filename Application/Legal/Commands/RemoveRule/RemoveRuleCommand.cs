namespace Poplike.Application.Legal.Commands.RemoveRule;

public class RemoveRuleCommand : IRemoveRuleCommand
{
    private readonly IDatabaseService _database;

    public RemoveRuleCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(
        IUserToken userToken, RemoveRuleCommandModel model)
    {
        if (!userToken.CanRemoveRule())
            throw new NotPermittedException();

        if (!model.Confirmed)
            throw new ConfirmationRequiredException();

        var rule = await _database.Rules
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        _database.Rules.Remove(rule);

        await _database.SaveAsync(userToken);
    }
}
