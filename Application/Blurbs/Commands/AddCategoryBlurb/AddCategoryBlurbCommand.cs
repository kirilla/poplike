using Poplike.Application.Legal.Filters;

namespace Poplike.Application.Blurbs.Commands.AddCategoryBlurb;

public class AddCategoryBlurbCommand : IAddCategoryBlurbCommand
{
    private readonly IDatabaseService _database;
    private readonly IWordPreventionFilter _filter;

    public AddCategoryBlurbCommand(
        IDatabaseService database,
        IWordPreventionFilter filter)
    {
        _database = database;
        _filter = filter;
    }

    public async Task<int> Execute(
        IUserToken userToken, AddCategoryBlurbCommandModel model)
    {
        if (!userToken.CanAddCategoryBlurb())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        var user = await _database.Users
            .Where(x => x.Id == userToken.UserId!.Value)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var category = await _database.Categories
            .Where(x => x.Id == model.CategoryId!.Value)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var blurb = new CategoryBlurb()
        {
            CategoryId = category.Id,
            Text = model.Text,
        };

        _database.CategoryBlurbs.Add(blurb);

        await _filter.Filter(model.Text);

        await _database.SaveAsync(userToken);

        return blurb.Id;
    }
}
