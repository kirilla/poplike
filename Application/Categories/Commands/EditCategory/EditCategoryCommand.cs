namespace Poplike.Application.Categories.Commands.EditCategory;

public class EditCategoryCommand : IEditCategoryCommand
{
    private readonly IDatabaseService _database;

    public EditCategoryCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(
        IUserToken userToken, EditCategoryCommandModel model)
    {
        if (!userToken.CanEditCategory())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        if (await _database.Categories.AnyAsync(x => 
            x.Name == model.Name && 
            x.Id != model.Id))
            throw new BlockedByExistingException();

        var category = await _database.Categories
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        category.Emoji = model.Emoji;
        category.Name = model.Name;

        category.SubjectHeading = model.SubjectHeading;
        category.SubjectPlaceholder = model.SubjectPlaceholder;

        category.ExpressionSetId = model.ExpressionSetId!.Value;

        await _database.SaveAsync(userToken);
    }
}
