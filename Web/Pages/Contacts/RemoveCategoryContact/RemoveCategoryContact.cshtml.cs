using Poplike.Application.Contacts.Commands.RemoveCategoryContact;

namespace Poplike.Web.Pages.Contacts.RemoveCategoryContact;

public class RemoveCategoryContactModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IRemoveCategoryContactCommand _command;

    public CategoryContact Contact { get; set; }
    public Category Category { get; set; }

    [BindProperty]
    public RemoveCategoryContactCommandModel CommandModel { get; set; }

    public RemoveCategoryContactModel(
        IUserToken userToken,
        IDatabaseService database,
        IRemoveCategoryContactCommand command)
        : base(PageKind.RemoveCategoryContact, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanRemoveCategoryContact())
                throw new NotPermittedException();

            Contact = await _database.CategoryContacts
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Category = await _database.Categories
                .Where(x => x.Id == Contact.CategoryId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new RemoveCategoryContactCommandModel()
            {
                Id = Contact.Id,
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
            if (!UserToken.CanRemoveCategoryContact())
                throw new NotPermittedException();

            Contact = await _database.CategoryContacts
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Category = await _database.Categories
                .Where(x => x.Id == Contact.CategoryId)
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
