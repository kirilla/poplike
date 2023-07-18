using Poplike.Application.Statements.Reactions.ReorderStatements;

namespace Poplike.Application.Statements.Commands.RemoveStatement;

public class RemoveStatementCommand : IRemoveStatementCommand
{
    private readonly IDatabaseService _database;
    private readonly IReorderStatementsReaction _reaction;

    public RemoveStatementCommand(
        IDatabaseService database,
        IReorderStatementsReaction reaction)
    {
        _database = database;
        _reaction = reaction;
    }

    public async Task Execute(
        IUserToken userToken, RemoveStatementCommandModel model)
    {
        if (!userToken.CanRemoveStatement())
            throw new NotPermittedException();

        if (!model.Confirmed)
            throw new ConfirmationRequiredException();

        var statement = await _database.Statements
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var subjectId = statement.SubjectId;

        _database.Statements.Remove(statement);

        await _database.SaveAsync(userToken);

        await _reaction.Execute(subjectId);
    }
}
