using Poplike.Application.Statements.Reactions.CountUserStatements;
using Poplike.Application.Statements.Reactions.PruneUserStatements;

namespace Poplike.Application.Statements.Commands.ToggleUserStatement;

public class ToggleUserStatementCommand : IToggleUserStatementCommand
{
    private readonly IDatabaseService _database;
    private readonly ICountUserStatementsReaction _countReaction;
    private readonly IPruneUserStatementsReaction _pruneReaction;

    public ToggleUserStatementCommand(
        IDatabaseService database,
        ICountUserStatementsReaction countReaction,
        IPruneUserStatementsReaction pruneReaction)
    {
        _database = database;
        _countReaction = countReaction;
        _pruneReaction = pruneReaction;
    }

    public async Task Execute(IUserToken userToken, ToggleUserStatementCommandModel model)
    {
        if (!userToken.CanToggleUserStatement())
            throw new NotPermittedException();

        var statement = await _database.Statements
            .Where(x => x.Id == model.StatementId)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var userStatements = await _database.UserStatements
            .Where(x =>
                x.StatementId == model.StatementId &&
                x.UserId == userToken.UserId!.Value)
            .ToListAsync();

        if (userStatements.Any())
        {
            _database.UserStatements.RemoveRange(userStatements);
        }
        else
        {
            var userStatement = new UserStatement()
            {
                UserId = userToken.UserId!.Value,
                StatementId = statement.Id,
            };

            _database.UserStatements.Add(userStatement);

            var subject = await _database.Subjects
                .Where(x => x.Id == statement.SubjectId)
                .Select(x => new { 
                    x.MultipleChoice,
                })
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            if (!subject.MultipleChoice)
            {
                var otherUserStatements = await _database.UserStatements
                    .Where(x =>
                        x.Statement.SubjectId == statement.SubjectId &&
                        x.StatementId != statement.Id)
                    .ToListAsync();

                _database.UserStatements.RemoveRange(otherUserStatements);
            }
        }

        await _database.SaveAsync(userToken);

        await _countReaction.Execute(statement.SubjectId);
        await _pruneReaction.Execute(statement.SubjectId);
    }
}
