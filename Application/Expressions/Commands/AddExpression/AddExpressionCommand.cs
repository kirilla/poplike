using Poplike.Application.Expressions.Reactions.ReorderExpressions;
using Poplike.Application.Legal.Filters;

namespace Poplike.Application.Expressions.Commands.AddExpression;

public class AddExpressionCommand : IAddExpressionCommand
{
    private readonly IDatabaseService _database;
    private readonly IWordPreventionFilter _filter;
    private readonly IReorderExpressionsReaction _reaction;

    public AddExpressionCommand(
        IDatabaseService database,
        IWordPreventionFilter filter,
        IReorderExpressionsReaction reaction)
    {
        _database = database;
        _filter = filter;
        _reaction = reaction;
    }

    public async Task<int> Execute(
        IUserToken userToken, AddExpressionCommandModel model)
    {
        if (!userToken.CanAddExpression())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        var user = await _database.Users
            .Where(x => x.Id == userToken.UserId!.Value)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var set = await _database.ExpressionSets
            .Where(x => x.Id == model.ExpressionSetId!.Value)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var expressions = await _database.Expressions
            .Where(x => x.ExpressionSetId == model.ExpressionSetId!.Value)
            .ToListAsync();

        if (expressions.Any(x => x.Characters == model.Characters))
            throw new BlockedByExistingException();

        var expression = new Expression()
        {
            Characters = model.Characters,
            ExpressionSetId = set.Id,
            Order = int.MaxValue,
        };

        _database.Expressions.Add(expression);

        await _filter.Filter(model.Characters);

        await _database.SaveAsync(userToken);

        await _reaction.Execute(set.Id);

        return expression.Id;
    }
}
