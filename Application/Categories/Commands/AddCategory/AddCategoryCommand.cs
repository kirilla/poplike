namespace Poplike.Application.Categories.Commands.AddCategory;

public class AddCategoryCommand : IAddCategoryCommand
{
    private readonly IDatabaseService _database;

    public AddCategoryCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task<int> Execute(
        IUserToken userToken, AddCategoryCommandModel model)
    {
        if (!userToken.CanAddCategory())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        var expressionGroup = await _database.ExpressionSets
            .Where(x => x.Id == model.ExpressionSetId.Value)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        if (await _database.Categories.AnyAsync(x => x.Name == model.Name))
            throw new BlockedByExistingException();

        var category = new Category()
        {
            Emoji = model.Emoji,
            Name = model.Name,
            SubjectHeading = model.SubjectHeading,
            SubjectPlaceholder = model.SubjectPlaceholder,
            ExpressionSetId = model.ExpressionSetId!.Value,
        };
        
        _database.Categories.Add(category);

        var expressions = await _database.Expressions
            .AsNoTracking()
            .Where(x => x.ExpressionSetId == model.ExpressionSetId)
            .ToListAsync();

        await _database.SaveAsync(userToken);

        return category.Id;
    }
}
