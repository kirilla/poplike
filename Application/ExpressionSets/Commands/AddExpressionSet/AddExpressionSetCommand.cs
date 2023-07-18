using Poplike.Application.Legal.Filters;

namespace Poplike.Application.ExpressionSets.Commands.AddExpressionSet;

public class AddExpressionSetCommand : IAddExpressionSetCommand
{
    private readonly IDatabaseService _database;
    private readonly IWordPreventionFilter _filter;

    public AddExpressionSetCommand(
        IDatabaseService database,
        IWordPreventionFilter filter)
    {
        _database = database;
        _filter = filter;
    }

    public async Task<int> Execute(
        IUserToken userToken, AddExpressionSetCommandModel model)
    {
        if (!userToken.CanAddExpressionSet())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        var list = new List<string?>()
        {
            model.Expression1,
            model.Expression2,
            model.Expression3,
            model.Expression4,
            model.Expression5,
        }
        .Where(x => !string.IsNullOrWhiteSpace(x))
        .Cast<string>()
        .ToList();

        foreach (var item in list)
        {
            await _filter.Filter(item);
        }

        if (await _database.ExpressionSets.AnyAsync(x => x.Name == model.Name))
            throw new BlockedByExistingException();

        var set = new ExpressionSet()
        {
            Emoji = model.Emoji,
            Name = model.Name,
            MultipleChoice = model.MultipleChoice,
            FreeExpression = model.FreeExpression,
        };

        _database.ExpressionSets.Add(set);

        int i = 0;

        var expressions = list
            .Select(x => new Expression()
            {
                Characters = x,
                ExpressionSet = set,
                Order = i += 1,
            })
            .ToList();

        _database.Expressions.AddRange(expressions);

        await _database.SaveAsync(userToken);

        return set.Id;
    }
}
