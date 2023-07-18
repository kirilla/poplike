using Poplike.Application.Legal.Filters;

namespace Poplike.Application.Blurbs.Commands.EditCategoryBlurb;

public class EditCategoryBlurbCommand : IEditCategoryBlurbCommand
{
    private readonly IDatabaseService _database;
    private readonly IWordPreventionFilter _filter;

    public EditCategoryBlurbCommand(
        IDatabaseService database,
        IWordPreventionFilter filter)
    {
        _database = database;
        _filter = filter;
    }

    public async Task Execute(
        IUserToken userToken, EditCategoryBlurbCommandModel model)
    {
        if (!userToken.CanEditCategoryBlurb())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        var blurb = await _database.CategoryBlurbs
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        blurb.Text = model.Text;

        await _filter.Filter(model.Text);

        await _database.SaveAsync(userToken);
    }
}
