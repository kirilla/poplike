using Poplike.Application.Statements.Commands.RemoveStatement;

namespace Poplike.Web.Pages.Statements.RemoveStatement;

public class RemoveStatementModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IRemoveStatementCommand _command;

    public Statement Statement { get; set; }
    public Subject Subject { get; set; }

    public List<UserStatement> UserStatements { get; set; }

    [BindProperty]
    public RemoveStatementCommandModel CommandModel { get; set; }

    public RemoveStatementModel(
        IUserToken userToken,
        IDatabaseService database,
        IRemoveStatementCommand command)
        : base(PageKind.RemoveStatement, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanRemoveStatement())
                throw new NotPermittedException();

            Statement = await _database.Statements
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Subject = await _database.Subjects
                .Include(x => x.Category)
                .Where(x => x.Id == Statement.SubjectId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new RemoveStatementCommandModel()
            {
                Id = Statement.Id,
            };

            return Page();
        }
        catch (NotFoundException)
        {
            return Redirect("/help/notfound");
        }
        catch (Exception ex)
        {
            return Redirect("/help/notpermitted");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!UserToken.CanRemoveStatement())
                throw new NotPermittedException();

            Statement = await _database.Statements
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Subject = await _database.Subjects
                .Include(x => x.Category)
                .Where(x => x.Id == Statement.SubjectId)
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
        catch (Exception ex)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
