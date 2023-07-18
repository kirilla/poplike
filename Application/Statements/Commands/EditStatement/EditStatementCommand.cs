using Poplike.Application.Legal.Filters;

namespace Poplike.Application.Statements.Commands.EditStatement;

public class EditStatementCommand : IEditStatementCommand
{
    private readonly IDatabaseService _database;
    private readonly IWordPreventionFilter _filter;

    public EditStatementCommand(
        IDatabaseService database,
        IWordPreventionFilter filter)
    {
        _database = database;
        _filter = filter;
    }

    public async Task Execute(
        IUserToken userToken, EditStatementCommandModel model)
    {
        if (!userToken.CanEditStatement())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();
        
        var statement = await _database.Statements
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        if (statement.UserCreated)
            throw new NotPermittedException();

        var statements = await _database.Statements
            .Where(x => x.SubjectId == statement.SubjectId)
            .ToListAsync();

        if (statements.Any(x =>
            x.Sentence == model.Sentence &&
            x.Id != model.Id))
            throw new BlockedByExistingException();

        statement.Sentence = model.Sentence;

        await _filter.Filter(model.Sentence);

        await _database.SaveAsync(userToken);
    }
}
