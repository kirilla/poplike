namespace Poplike.Application.ExpressionSets.Commands.RemoveExpressionSet;

public class RemoveExpressionSetCommand : IRemoveExpressionSetCommand
{
    private readonly IDatabaseService _database;

    public RemoveExpressionSetCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(
        IUserToken userToken, RemoveExpressionSetCommandModel model)
    {
        if (!userToken.CanRemoveExpressionSet())
            throw new NotPermittedException();

        if (!model.Confirmed)
            throw new ConfirmationRequiredException();

        var set = await _database.ExpressionSets
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        _database.ExpressionSets.Remove(set);

        await _database.SaveAsync(userToken);
    }
}
