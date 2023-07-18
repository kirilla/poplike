namespace Poplike.Application.Expressions.Commands.RemoveExpression;

public class RemoveExpressionCommand : IRemoveExpressionCommand
{
    private readonly IDatabaseService _database;

    public RemoveExpressionCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(
        IUserToken userToken, RemoveExpressionCommandModel model)
    {
        if (!userToken.CanRemoveExpression())
            throw new NotPermittedException();

        if (!model.Confirmed)
            throw new ConfirmationRequiredException();

        var expression = await _database.Expressions
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        _database.Expressions.Remove(expression);

        await _database.SaveAsync(userToken);
    }
}
