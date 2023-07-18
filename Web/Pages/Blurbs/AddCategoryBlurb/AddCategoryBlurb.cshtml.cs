using Poplike.Application.Blurbs.Commands.AddCategoryBlurb;

namespace Poplike.Web.Pages.Blurbs.AddCategoryBlurb;

public class AddCategoryBlurbModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IAddCategoryBlurbCommand _command;

    public Category Category { get; set; }

    [BindProperty]
    public AddCategoryBlurbCommandModel CommandModel { get; set; }

    public AddCategoryBlurbModel(
        IUserToken userToken,
        IDatabaseService database,
        IAddCategoryBlurbCommand command)
        : base(PageKind.AddCategoryBlurb, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanAddCategoryBlurb())
                throw new NotPermittedException();

            Category = await _database.Categories
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new AddCategoryBlurbCommandModel()
            {
                CategoryId = Category.Id,
            };

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
            if (!UserToken.CanAddCategoryBlurb())
                throw new NotPermittedException();

            Category = await _database.Categories
                .Where(x => x.Id == CommandModel.CategoryId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/category/curate/{Category.Id}");
        }
        catch (WordPreventedException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Text),
                "Texten innehåller ett blockerat ord eller symbol.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
