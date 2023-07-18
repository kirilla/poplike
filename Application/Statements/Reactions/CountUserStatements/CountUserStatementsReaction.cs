namespace Poplike.Application.Statements.Reactions.CountUserStatements;

public class CountUserStatementsReaction : ICountUserStatementsReaction
{
    private readonly IDatabaseService _database;

    public CountUserStatementsReaction(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(int subjectId)
    {
        var subject = await _database.Subjects
            .Where(x => x.Id == subjectId)
            .SingleOrDefaultAsync();

        var count = await _database.UserStatements
            .CountAsync(x => x.Statement.SubjectId == subjectId);

        subject.StatementCount = count;

        await _database.SaveAsync(new NoUserToken());
    }
}
