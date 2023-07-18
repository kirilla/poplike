using Poplike.Application.Legal.Filters;
using Poplike.Application.Statements.Reactions.ReorderStatements;

namespace Poplike.Application.Statements.Commands.AddStatement;

public class AddStatementCommand : IAddStatementCommand
{
    private readonly IDatabaseService _database;
    private readonly IWordPreventionFilter _filter;
    private readonly IReorderStatementsReaction _reaction;

    public AddStatementCommand(
        IDatabaseService database,
        IWordPreventionFilter filter,
        IReorderStatementsReaction reaction)
    {
        _database = database;
        _filter = filter;
        _reaction = reaction;
    }

    public async Task Execute(
        IUserToken userToken, AddStatementCommandModel model)
    {
        if (!userToken.CanAddStatement())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        var subject = await _database.Subjects
            .Where(x => x.Id == model.SubjectId!.Value)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var statements = await _database.Statements
            .Where(x => x.SubjectId == model.SubjectId!.Value)
            .ToListAsync();

        if (statements.Any(x => x.Sentence == model.Sentence))
            throw new BlockedByExistingException();

        var statement = new Statement()
        {
            SubjectId = subject.Id,
            Sentence = model.Sentence,
            Order = int.MaxValue,
        };

        _database.Statements.Add(statement);

        await _filter.Filter(model.Sentence);

        await _database.SaveAsync(userToken);

        await _reaction.Execute(subject.Id);
    }
}
