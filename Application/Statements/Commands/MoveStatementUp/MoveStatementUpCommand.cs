namespace Poplike.Application.Statements.Commands.MoveStatementUp;

public class MoveStatementUpCommand : IMoveStatementUpCommand
{
    private readonly IDatabaseService _database;

    public MoveStatementUpCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(
        IUserToken userToken, MoveStatementUpCommandModel model)
    {
        if (!userToken.CanMoveStatementUp())
            throw new NotPermittedException();

        var statement = await _database.Statements
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var statements = await _database.Statements
            .Where(x => x.SubjectId == statement.SubjectId)
            .OrderBy(x => x.Order)
            .ToListAsync();

        int i = 0;

        statements
            .OrderBy(x => x.Order)
            .ToList()
            .ForEach(x => x.Order = i += 1);

        var itemToMove = statements
            .Where(x => x.Id == model.Id)
            .SingleOrDefault() ??
            throw new NotFoundException();

        itemToMove.Order -= 1;

        var itemsToDisplace = statements
            .Where(x => 
                x.Order == itemToMove.Order && 
                x.Id != itemToMove.Id)
            .ToList();

        itemsToDisplace.ForEach(x => x.Order += 1);

        i = 0;

        statements
            .OrderBy(x => x.Order)
            .ToList()
            .ForEach(x => x.Order = i += 1);

        await _database.SaveAsync(userToken);
    }
}
