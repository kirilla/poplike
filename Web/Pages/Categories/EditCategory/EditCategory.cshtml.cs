using Poplike.Application.Categories.Commands.EditCategory;

namespace Poplike.Web.Pages.Categories.EditCategory;

public class EditCategoryModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IEditCategoryCommand _command;

    public List<ExpressionSet> ExpressionSets { get; set; }

    [BindProperty]
    public EditCategoryCommandModel CommandModel { get; set; }

    public EditCategoryModel(
        IUserToken userToken,
        IDatabaseService database,
        IEditCategoryCommand command)
        : base(PageKind.EditCategory, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanEditCategory())
                throw new NotPermittedException();

            var category = await _database.Categories
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new EditCategoryCommandModel()
            {
                Id = category.Id,
                Emoji = category.Emoji,
                Name = category.Name,
                SubjectHeading = category.SubjectHeading,
                SubjectPlaceholder = category.SubjectPlaceholder,
                ExpressionSetId = category.ExpressionSetId,
            };

            ExpressionSets = await _database.ExpressionSets
                .OrderBy(x => x.Name)
                .ToListAsync();

            return Page();
        }
        catch (BlockedByExistingException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Name),
                "Det finns redan en sådan kategori.");

            return Page();
        }
        catch (NotFoundException)
        {
            return Redirect("/help/notfound");
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!UserToken.CanEditCategory())
                throw new NotPermittedException();

            ExpressionSets = await _database.ExpressionSets
                .OrderBy(x => x.Name)
                .ToListAsync();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/category/curate/{CommandModel.Id}");
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
