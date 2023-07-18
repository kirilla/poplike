using Poplike.Application.Categories.Commands.AddCategory;

namespace Poplike.Web.Pages.Categories.AddCategory;

public class AddCategoryModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IAddCategoryCommand _command;

    public List<ExpressionSet> ExpressionSets { get; set; }

    [BindProperty]
    public AddCategoryCommandModel CommandModel { get; set; }

    public AddCategoryModel(
        IUserToken userToken,
        IDatabaseService database,
        IAddCategoryCommand command)
        : base(PageKind.AddCategory, userToken)
    {
        _database = database;
        _command = command;
    }
    
    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanAddCategory())
                throw new NotPermittedException();

            ExpressionSets = await _database.ExpressionSets
                .OrderBy(x => x.Name)
                .ToListAsync();

            CommandModel = new AddCategoryCommandModel();

            return Page();
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
            if (!UserToken.CanAddCategory())
                throw new NotPermittedException();

            ExpressionSets = await _database.ExpressionSets
                .OrderBy(x => x.Name)
                .ToListAsync();

            if (!ModelState.IsValid)
                return Page();

            var id = await _command.Execute(UserToken, CommandModel);

            return Redirect($"/category/show/{id}");
        }
        catch (BlockedByExistingException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Name),
                "Kategorin finns redan.");

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
}
