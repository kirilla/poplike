using Poplike.Application.Statements.Commands.MoveStatementUp;

namespace Poplike.Web.Pages.Statements.MoveStatementUp;

public class MoveStatementUpModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IMoveStatementUpCommand _command;

    public Statement Statement { get; set; }

    [BindProperty]
    public MoveStatementUpCommandModel CommandModel { get; set; }

    public MoveStatementUpModel(
        IUserToken userToken,
        IDatabaseService database,
        IMoveStatementUpCommand command)
        : base(PageKind.MoveStatementUp, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanMoveStatementUp())
                throw new NotPermittedException();

            Statement = await _database.Statements
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new MoveStatementUpCommandModel()
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
            if (!UserToken.CanMoveStatementUp())
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
