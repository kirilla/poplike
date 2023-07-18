using Poplike.Application.Legal.Filters;

namespace Poplike.Application.Expressions.Commands.EditExpression;

public class EditExpressionCommand : IEditExpressionCommand
{
    private readonly IDatabaseService _database;
    private readonly IWordPreventionFilter _filter;

    public EditExpressionCommand(
        IDatabaseService database,
        IWordPreventionFilter filter)
    {
        _database = database;
        _filter = filter;
    }

    public async Task Execute(
        IUserToken userToken, EditExpressionCommandModel model)
    {
        if (!userToken.CanEditExpression())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();
        
        var expression = await _database.Expressions
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var expressions = await _database.Expressions
            .Where(x => x.ExpressionSetId == expression.ExpressionSetId)
            .ToListAsync();

        if (expressions.Any(x =>
            x.Characters == model.Characters &&
            x.Id != model.Id))
            throw new BlockedByExistingException();

        expression.Characters = model.Characters;

        await _filter.Filter(model.Characters);

        await _database.SaveAsync(userToken);
    }
}
