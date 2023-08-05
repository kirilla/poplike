using Poplike.Application.Subjects.Commands.EditSubject;

namespace Poplike.Web.Pages.Subjects.EditSubject;

public class EditSubjectModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IEditSubjectCommand _command;

    public Subject Subject { get; set; }

    [BindProperty]
    public EditSubjectCommandModel CommandModel { get; set; }

    public EditSubjectModel(
        IUserToken userToken,
        IDatabaseService database,
        IEditSubjectCommand command)
        :
        base(PageKind.EditSubject, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanEditSubject())
                throw new NotPermittedException();

            Subject = await _database.Subjects
                .Include(x => x.Category)
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new EditSubjectCommandModel()
            {
                Id = Subject.Id,
                Name = Subject.Name,
                MultipleChoice = Subject.MultipleChoice,
                FreeExpression = Subject.FreeExpression,
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
            if (!UserToken.CanEditSubject())
                throw new NotPermittedException();

            Subject = await _database.Subjects
                .Include(x => x.Category)
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/subject/curate/{CommandModel.Id}");
        }
        catch (BlockedByExistingException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Name),
                "Det här finns redan.");

            return Page();
        }
        catch (WordPreventedException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Name),
                "Namnet innehåller ett blockerat ord.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
