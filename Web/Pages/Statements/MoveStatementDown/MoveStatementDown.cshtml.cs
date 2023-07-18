using Poplike.Application.Statements.Commands.MoveStatementDown;

namespace Poplike.Web.Pages.Statements.MoveStatementDown;

public class MoveStatementDownModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IMoveStatementDownCommand _command;

    public Statement Statement { get; set; }

    [BindProperty]
    public MoveStatementDownCommandModel CommandModel { get; set; }

    public MoveStatementDownModel(
        IUserToken userToken,
        IDatabaseService database,
        IMoveStatementDownCommand command)
        : base(PageKind.MoveStatementDown, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanMoveStatementDown())
                throw new NotPermittedException();

            Statement = await _database.Statements
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new MoveStatementDownCommandModel()
            {
                Id = Statement.Id,
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
            if (!UserToken.CanMoveStatementDown())
                throw new NotPermittedException();

            Statement = await _database.Statements
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/subject/curate/{Statement.SubjectId}");
        }
        catch (Exception ex)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
