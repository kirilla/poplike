using Poplike.Application.Blurbs.Commands.RemoveCategoryBlurb;

namespace Poplike.Web.Pages.Blurbs.RemoveCategoryBlurb;

public class RemoveCategoryBlurbModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IRemoveCategoryBlurbCommand _command;

    public CategoryBlurb CategoryBlurb { get; set; }
    public Category Category { get; set; }

    [BindProperty]
    public RemoveCategoryBlurbCommandModel CommandModel { get; set; }

    public RemoveCategoryBlurbModel(
        IUserToken userToken,
        IDatabaseService database,
        IRemoveCategoryBlurbCommand command)
        : base(PageKind.RemoveCategoryBlurb, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanRemoveCategoryBlurb())
                throw new NotPermittedException();

            CategoryBlurb = await _database.CategoryBlurbs
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Category = await _database.Categories
                .Where(x => x.Id == CategoryBlurb.CategoryId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new RemoveCategoryBlurbCommandModel()
            {
                Id = CategoryBlurb.Id,
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
            if (!UserToken.CanRemoveCategoryBlurb())
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
        catch (ConfirmationRequiredException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Confirmed),
                "Bekräfta att du verkligen vill ta bort uttrycket.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
