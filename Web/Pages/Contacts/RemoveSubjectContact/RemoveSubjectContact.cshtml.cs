using Poplike.Application.Contacts.Commands.RemoveSubjectContact;

namespace Poplike.Web.Pages.Contacts.RemoveSubjectContact;

public class RemoveSubjectContactModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IRemoveSubjectContactCommand _command;

    public SubjectContact Contact { get; set; }
    public Subject Subject { get; set; }

    [BindProperty]
    public RemoveSubjectContactCommandModel CommandModel { get; set; }

    public RemoveSubjectContactModel(
        IUserToken userToken,
        IDatabaseService database,
        IRemoveSubjectContactCommand command)
        : base(PageKind.RemoveSubjectContact, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanRemoveSubjectContact())
                throw new NotPermittedException();

            Contact = await _database.SubjectContacts
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Subject = await _database.Subjects
                .Where(x => x.Id == Contact.SubjectId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new RemoveSubjectContactCommandModel()
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
            if (!UserToken.CanRemoveSubjectContact())
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
