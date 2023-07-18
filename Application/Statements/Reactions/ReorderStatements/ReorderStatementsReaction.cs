namespace Poplike.Application.Statements.Reactions.ReorderStatements;

public class ReorderStatementsReaction : IReorderStatementsReaction
{
    private readonly IDatabaseService _database;

    public ReorderStatementsReaction(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(int subjectId)
    {
        var statements = await _database.Statements
            .Where(x => x.SubjectId == subjectId)
            .ToListAsync();

        int i = 0;

        statements
            .OrderBy(x => x.Order)
            .ThenBy(x => x.Created)
            .ToList()
            .ForEach(x => x.Order = i += 1);

        await _database.SaveAsync(new NoUserToken());
    }
}
