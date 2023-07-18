namespace Poplike.Application.ExpressionSets.Commands.EditExpressionSet;

public class EditExpressionSetCommand : IEditExpressionSetCommand
{
    private readonly IDatabaseService _database;

    public EditExpressionSetCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(
        IUserToken userToken, EditExpressionSetCommandModel model)
    {
        if (!userToken.CanEditExpressionSet())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        if (await _database.ExpressionSets.AnyAsync(x => 
            x.Name == model.Name && 
            x.Id != model.Id))
            throw new BlockedByExistingException();

        var set = await _database.ExpressionSets
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        set.Emoji = model.Emoji;
        set.Name = model.Name;
        set.MultipleChoice = model.MultipleChoice;
        set.FreeExpression = model.FreeExpression;

        await _database.SaveAsync(userToken);
    }
}
