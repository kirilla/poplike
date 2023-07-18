using Poplike.Application.Statements.Commands.AddStatement;

namespace Poplike.Web.Pages.Statements.AddStatement;

public class AddStatementModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IAddStatementCommand _command;

    public Subject Subject { get; set; }

    [BindProperty]
    public AddStatementCommandModel CommandModel { get; set; }

    public AddStatementModel(
        IUserToken userToken,
        IDatabaseService database,
        IAddStatementCommand command)
        : base(PageKind.AddStatement, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanAddStatement())
                throw new NotPermittedException();

            Subject = await _database.Subjects
                .Include(x => x.Category)
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new AddStatementCommandModel()
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
            if (!UserToken.CanAddStatement())
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
                nameof(CommandModel.Sentence),
                "Det finns redan ett sådant uttryck.");

            return Page();
        }
        catch (WordPreventedException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Sentence),
                "Uttrycket innehåller ett blockerat ord eller symbol.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
