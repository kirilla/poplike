namespace Poplike.Application.Statements.Reactions.PruneUserStatements;

public class PruneUserStatementsReaction : IPruneUserStatementsReaction
{
    private readonly IDatabaseService _database;

    public PruneUserStatementsReaction(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(int subjectId)
    {
        var statements = await _database.Statements
            .Where(x => 
                x.SubjectId == subjectId &&
                x.UserCreated == true &&
                x.UserStatements.Count() == 0)
            .ToListAsync();

        _database.Statements.RemoveRange(statements);

        await _database.SaveAsync(new NoUserToken());
    }
}
