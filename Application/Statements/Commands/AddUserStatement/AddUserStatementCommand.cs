using Poplike.Application.Statements.Reactions.CountUserStatements;
using Poplike.Application.Statements.Reactions.PruneUserStatements;

namespace Poplike.Application.Statements.Commands.AddUserStatement;

public class AddUserStatementCommand : IAddUserStatementCommand
{
    private readonly IDatabaseService _database;
    private readonly ICountUserStatementsReaction _countReaction;
    private readonly IPruneUserStatementsReaction _pruneReaction;

    public AddUserStatementCommand(
        IDatabaseService database,
        ICountUserStatementsReaction countReaction,
        IPruneUserStatementsReaction pruneReaction)
    {
        _database = database;
        _countReaction = countReaction;
        _pruneReaction = pruneReaction;
    }

    public async Task Execute(IUserToken userToken, AddUserStatementCommandModel model)
    {
        if (!userToken.CanAddUserStatement())
            throw new NotPermittedException();

        var subject = await _database.Subjects
            .Where(x => x.Id == model.SubjectId)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var statements = await _database.Statements
            .Where(x => x.SubjectId == model.SubjectId!.Value)
            .ToListAsync();

        if (statements.Any(x => x.Sentence == model.Sentence))
            throw new BlockedByExistingException();

        var userStatements = await _database.UserStatements
            .Where(x =>
                x.Statement.SubjectId == model.SubjectId!.Value &&
                x.UserId == userToken.UserId!.Value)
            .ToListAsync();

        var statement = new Statement()
        {
            SubjectId = model.SubjectId!.Value,
            Sentence = model.Sentence!,
            Order = int.MaxValue,
            UserCreated = true,
        };

        _database.Statements.Add(statement);

        var userStatement = new UserStatement()
        {
            UserId = userToken.UserId!.Value,
            Statement = statement,
        };

        _database.UserStatements.Add(userStatement);

        if (!subject.MultipleChoice)
        {
            _database.UserStatements.RemoveRange(userStatements);
        }

        await _database.SaveAsync(userToken);

        await _countReaction.Execute(statement.SubjectId);
        await _pruneReaction.Execute(statement.SubjectId);
    }
}
