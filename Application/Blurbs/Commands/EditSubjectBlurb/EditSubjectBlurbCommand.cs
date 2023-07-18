using Poplike.Application.Legal.Filters;

namespace Poplike.Application.Blurbs.Commands.EditSubjectBlurb;

public class EditSubjectBlurbCommand : IEditSubjectBlurbCommand
{
    private readonly IDatabaseService _database;
    private readonly IWordPreventionFilter _filter;

    public EditSubjectBlurbCommand(
        IDatabaseService database,
        IWordPreventionFilter filter)
    {
        _database = database;
        _filter = filter;
    }

    public async Task Execute(
        IUserToken userToken, EditSubjectBlurbCommandModel model)
    {
        if (!userToken.CanEditSubjectBlurb())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        var blurb = await _database.SubjectBlurbs
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        blurb.Text = model.Text;

        await _filter.Filter(model.Text);

        await _database.SaveAsync(userToken);
    }
}
