using Poplike.Application.Legal.Filters;

namespace Poplike.Application.Keywords.Commands.AddKeyword;

public class AddKeywordCommand : IAddKeywordCommand
{
    private readonly IDatabaseService _database;
    private readonly IWordPreventionFilter _filter;

    public AddKeywordCommand(
        IDatabaseService database,
        IWordPreventionFilter filter)
    {
        _database = database;
        _filter = filter;
    }

    public async Task<int> Execute(
        IUserToken userToken, AddKeywordCommandModel model)
    {
        if (!userToken.CanAddKeyword())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        var user = await _database.Users
            .Where(x => x.Id == userToken.UserId!.Value)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var subject = await _database.Subjects
            .Where(x => x.Id == model.SubjectId!.Value)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var keyword = new Keyword()
        {
            SubjectId = subject.Id,
            Word = model.Word,
        };

        _database.Keywords.Add(keyword);

        await _filter.Filter(model.Word);

        await _database.SaveAsync(userToken);

        return keyword.Id;
    }
}
