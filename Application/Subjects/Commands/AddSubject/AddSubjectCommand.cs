using Poplike.Application.Legal.Filters;

namespace Poplike.Application.Subjects.Commands.AddSubject;

public class AddSubjectCommand : IAddSubjectCommand
{
    private readonly IDatabaseService _database;
    private readonly IWordPreventionFilter _filter;

    public AddSubjectCommand(
        IDatabaseService database,
        IWordPreventionFilter filter)
    {
        _database = database;
        _filter = filter;
    }

    public async Task<int> Execute(IUserToken userToken, AddSubjectCommandModel model)
    {
        if (!userToken.CanAddSubject())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        var category = await _database.Categories
            .Where(x => x.Id == model.CategoryId!.Value)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var expressionSet = await _database.ExpressionSets
            .Where(x => x.Id == model.ExpressionSetId!.Value)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        if (await _database.Subjects
                .Where(x => x.Name == model.Name)
                .AnyAsync())
            throw new BlockedByExistingException();

        var subject = new Subject()
        {
            Name = model.Name,
            CategoryId = model.CategoryId!.Value,
            MultipleChoice = expressionSet.MultipleChoice,
            FreeExpression = expressionSet.FreeExpression,
        };

        _database.Subjects.Add(subject);

        var expressions = await _database.Expressions
            .AsNoTracking()
            .Where(x => x.ExpressionSetId == expressionSet.Id)
            .ToListAsync();

        var statements = expressions
            .Select(x => new Statement()
            {
                Subject = subject,
                Sentence = x.Characters,
                Order = x.Order,
            })
            .ToList();

        _database.Statements.AddRange(statements);

        await _filter.Filter(model.Name);

        await _database.SaveAsync(userToken);

        return subject.Id;
    }
}
