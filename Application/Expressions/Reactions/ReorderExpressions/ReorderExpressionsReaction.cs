namespace Poplike.Application.Expressions.Reactions.ReorderExpressions;

public class ReorderExpressionsReaction : IReorderExpressionsReaction
{
    private readonly IDatabaseService _database;

    public ReorderExpressionsReaction(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(int expressionSetId)
    {
        var expressions = await _database.Expressions
            .Where(x => x.ExpressionSetId == expressionSetId)
            .ToListAsync();

        int i = 0;

        expressions
            .OrderBy(x => x.Order)
            .ThenBy(x => x.Created)
            .ToList()
            .ForEach(x => x.Order = i += 1);

        await _database.SaveAsync(new NoUserToken());
    }
}
