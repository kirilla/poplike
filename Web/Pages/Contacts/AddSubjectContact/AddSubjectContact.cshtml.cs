using Poplike.Application.Contacts.Commands.AddSubjectContact;

namespace Poplike.Web.Pages.Contacts.AddSubjectContact;

public class AddSubjectContactModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IAddSubjectContactCommand _command;

    public Subject Subject { get; set; }

    [BindProperty]
    public AddSubjectContactCommandModel CommandModel { get; set; }

    public AddSubjectContactModel(
        IUserToken userToken,
        IDatabaseService database,
        IAddSubjectContactCommand command)
        : base(PageKind.AddSubjectContact, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanAddSubjectContact())
                throw new NotPermittedException();

            Subject = await _database.Subjects
                .Include(x => x.Category)
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new AddSubjectContactCommandModel()
            {
                SubjectId = Subject.Id,
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
            if (!UserToken.CanAddSubjectContact())
                throw new NotPermittedException();

            Subject = await _database.Subjects
                .Where(x => x.Id == CommandModel.SubjectId)
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
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
