using Poplike.Application.Statements.Reactions.CountUserStatements;
using Poplike.Application.Statements.Reactions.PruneUserStatements;

namespace Poplike.Application.Statements.Commands.DeleteUserStatement;

public class DeleteUserStatementCommand : IDeleteUserStatementCommand
{
    private readonly IDatabaseService _database;
    private readonly ICountUserStatementsReaction _countReaction;
    private readonly IPruneUserStatementsReaction _pruneReaction;

    public DeleteUserStatementCommand(
        IDatabaseService database,
        ICountUserStatementsReaction countReaction,
        IPruneUserStatementsReaction pruneReaction)
    {
        _database = database;
        _countReaction = countReaction;
        _pruneReaction = pruneReaction;
    }

    public async Task Execute(IUserToken userToken, DeleteUserStatementCommandModel model)
    {
        if (!userToken.CanDeleteUserStatement())
            throw new NotPermittedException();

        var userStatement = await _database.UserStatements
            .Include(x => x.Statement)
            .Where(x =>
                x.Id == model.UserStatementId &&
                x.UserId == userToken.UserId!.Value)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var subjectId = userStatement.Statement.SubjectId;

        _database.UserStatements.Remove(userStatement);

        await _database.SaveAsync(userToken);

        await _countReaction.Execute(subjectId);
        await _pruneReaction.Execute(subjectId);
    }
}
