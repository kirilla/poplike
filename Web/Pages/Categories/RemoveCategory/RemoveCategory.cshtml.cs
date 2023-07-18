using Poplike.Application.Categories.Commands.RemoveCategory;

namespace Poplike.Web.Pages.Categories.RemoveCategory;

public class RemoveCategoryModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IRemoveCategoryCommand _command;

    [BindProperty]
    public RemoveCategoryCommandModel CommandModel { get; set; }

    public RemoveCategoryModel(
        IUserToken userToken,
        IDatabaseService database,
        IRemoveCategoryCommand command)
        : base(PageKind.RemoveCategory, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanRemoveCategory())
                throw new NotPermittedException();

            var category = await _database.Categories
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new RemoveCategoryCommandModel()
            {
                Id = category.Id,
                Name = category.Name,
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
            if (!UserToken.CanRemoveCategory())
                throw new NotPermittedException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/subject/all");
        }
        catch (ConfirmationRequiredException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Confirmed),
                "Bekräfta att du verkligen vill ta bort kategorin.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
