namespace Poplike.Application.Legal.Commands.AddRule;

public class AddRuleCommand : IAddRuleCommand
{
    private readonly IDatabaseService _database;

    public AddRuleCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task<int> Execute(
        IUserToken userToken, AddRuleCommandModel model)
    {
        if (!userToken.CanAddRule())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        if (await _database.Rules
            .AnyAsync(x => x.Heading == model.Heading))
            throw new BlockedByExistingException();

        var rule = new Rule()
        {
            Number = model.Number,
            Heading = model.Heading,
            Text = model.Text,
        };

        _database.Rules.Add(rule);

        await _database.SaveAsync(userToken);

        return rule.Id;
    }
}
