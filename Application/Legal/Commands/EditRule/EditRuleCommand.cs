using NUnit.Framework.Internal.Execution;

namespace Poplike.Application.Legal.Commands.EditRule;

public class EditRuleCommand : IEditRuleCommand
{
    private readonly IDatabaseService _database;

    public EditRuleCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(
        IUserToken userToken, EditRuleCommandModel model)
    {
        if (!userToken.CanEditRule())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        if (await _database.Rules
            .AnyAsync(x => 
                x.Heading == model.Heading &&
                x.Id != model.Id))
            throw new BlockedByExistingException();

        var rule = await _database.Rules
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        rule.Number = model.Number;
        rule.Heading = model.Heading;
        rule.Text = model.Text;

        await _database.SaveAsync(userToken);
    }
}
