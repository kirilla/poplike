using Poplike.Application.Contacts.Commands.EditSubjectContact;

namespace Poplike.Web.Pages.Contacts.EditSubjectContact;

public class EditSubjectContactModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IEditSubjectContactCommand _command;

    public SubjectContact Contact { get; set; }
    public Subject Subject { get; set; }

    [BindProperty]
    public EditSubjectContactCommandModel CommandModel { get; set; }

    public EditSubjectContactModel(
        IUserToken userToken,
        IDatabaseService database,
        IEditSubjectContactCommand command)
        : base(PageKind.EditSubjectContact, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanEditSubjectContact())
                throw new NotPermittedException();

            Contact = await _database.SubjectContacts
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Subject = await _database.Subjects
                .Where(x => x.Id == Contact.SubjectId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new EditSubjectContactCommandModel()
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
            if (!UserToken.CanEditSubjectContact())
                throw new NotPermittedException();

            Contact = await _database.SubjectContacts
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Subject = await _database.Subjects
                .Where(x => x.Id == Contact.SubjectId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/subject/curate/{Subject.Id}");
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
