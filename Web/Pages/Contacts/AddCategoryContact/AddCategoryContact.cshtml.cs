using Poplike.Application.Contacts.Commands.AddCategoryContact;

namespace Poplike.Web.Pages.Contacts.AddCategoryContact;

public class AddCategoryContactModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IAddCategoryContactCommand _command;

    public Category Category { get; set; }

    [BindProperty]
    public AddCategoryContactCommandModel CommandModel { get; set; }

    public AddCategoryContactModel(
        IUserToken userToken,
        IDatabaseService database,
        IAddCategoryContactCommand command)
        : base(PageKind.AddCategoryContact, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanAddCategoryContact())
                throw new NotPermittedException();

            Category = await _database.Categories
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new AddCategoryContactCommandModel()
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
            if (!UserToken.CanAddCategoryContact())
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
        catch (BlockedByExistingException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Name),
                "Länken finns redan.");

            return Page();
        }
        catch (WordPreventedException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Name),
                "Länken innehåller ett blockerat ord eller symbol.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
