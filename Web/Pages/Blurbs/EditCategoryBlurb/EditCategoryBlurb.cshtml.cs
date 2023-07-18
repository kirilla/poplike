using Poplike.Application.Blurbs.Commands.EditCategoryBlurb;

namespace Poplike.Web.Pages.Blurbs.EditCategoryBlurb;

public class EditCategoryBlurbModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IEditCategoryBlurbCommand _command;

    public CategoryBlurb CategoryBlurb { get; set; }
    public Category Category { get; set; }

    [BindProperty]
    public EditCategoryBlurbCommandModel CommandModel { get; set; }

    public EditCategoryBlurbModel(
        IUserToken userToken,
        IDatabaseService database,
        IEditCategoryBlurbCommand command)
        : base(PageKind.EditCategoryBlurb, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanEditCategoryBlurb())
                throw new NotPermittedException();

            CategoryBlurb = await _database.CategoryBlurbs
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Category = await _database.Categories
                .Where(x => x.Id == CategoryBlurb.CategoryId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new EditCategoryBlurbCommandModel()
            {
                Id = CategoryBlurb.Id,
                Text = CategoryBlurb.Text,
            };

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
            if (!UserToken.CanEditCategoryBlurb())
                throw new NotPermittedException();

            CategoryBlurb = await _database.CategoryBlurbs
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Category = await _database.Categories
                .Where(x => x.Id == CategoryBlurb.CategoryId)
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
        catch (Exception ex)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
