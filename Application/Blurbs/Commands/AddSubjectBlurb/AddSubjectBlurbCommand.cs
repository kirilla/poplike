using Poplike.Application.Legal.Filters;

namespace Poplike.Application.Blurbs.Commands.AddSubjectBlurb;

public class AddSubjectBlurbCommand : IAddSubjectBlurbCommand
{
    private readonly IDatabaseService _database;
    private readonly IWordPreventionFilter _filter;

    public AddSubjectBlurbCommand(
        IDatabaseService database,
        IWordPreventionFilter filter)
    {
        _database = database;
        _filter = filter;
    }

    public async Task<int> Execute(
        IUserToken userToken, AddSubjectBlurbCommandModel model)
    {
        if (!userToken.CanAddSubjectBlurb())
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

        var blurb = new SubjectBlurb()
        {
            SubjectId = subject.Id,
            Text = model.Text,
        };

        _database.SubjectBlurbs.Add(blurb);

        await _filter.Filter(model.Text);

        await _database.SaveAsync(userToken);

        return blurb.Id;
    }
}
