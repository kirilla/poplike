using Poplike.Application.Contacts.Commands.EditCategoryContact;

namespace Poplike.Web.Pages.Contacts.EditCategoryContact;

public class EditCategoryContactModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IEditCategoryContactCommand _command;

    public CategoryContact Contact { get; set; }
    public Category Category { get; set; }

    [BindProperty]
    public EditCategoryContactCommandModel CommandModel { get; set; }

    public EditCategoryContactModel(
        IUserToken userToken,
        IDatabaseService database,
        IEditCategoryContactCommand command)
        : base(PageKind.EditCategoryContact, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanEditCategoryContact())
                throw new NotPermittedException();

            Contact = await _database.CategoryContacts
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Category = await _database.Categories
                .Where(x => x.Id == Contact.CategoryId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new EditCategoryContactCommandModel()
            {
                Id = Contact.Id,
                Name = Contact.Name,
                PhoneNumber = Contact.PhoneNumber,
                EmailAddress = Contact.EmailAddress,
                Url = Contact.Url,
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
            if (!UserToken.CanEditCategoryContact())
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
        catch (Exception ex)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
