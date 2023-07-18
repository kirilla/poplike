using Poplike.Application.Legal.Filters;

namespace Poplike.Application.Keywords.Commands.EditKeyword;

public class EditKeywordCommand : IEditKeywordCommand
{
    private readonly IDatabaseService _database;
    private readonly IWordPreventionFilter _filter;

    public EditKeywordCommand(
        IDatabaseService database,
        IWordPreventionFilter filter)
    {
        _database = database;
        _filter = filter;
    }

    public async Task Execute(
        IUserToken userToken, EditKeywordCommandModel model)
    {
        if (!userToken.CanEditKeyword())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        var keyword = await _database.Keywords
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        keyword.Word = model.Word;

        await _filter.Filter(model.Word);

        await _database.SaveAsync(userToken);
    }
}
