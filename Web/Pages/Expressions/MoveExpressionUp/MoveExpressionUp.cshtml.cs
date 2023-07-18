using Poplike.Application.Expressions.Commands.MoveExpressionUp;

namespace Poplike.Web.Pages.Expressions.MoveExpressionUp;

public class MoveExpressionUpModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IMoveExpressionUpCommand _command;

    public Expression Expression { get; set; }

    [BindProperty]
    public MoveExpressionUpCommandModel CommandModel { get; set; }

    public MoveExpressionUpModel(
        IUserToken userToken,
        IDatabaseService database,
        IMoveExpressionUpCommand command)
        : base(PageKind.MoveExpressionUp, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanMoveExpressionUp())
                throw new NotPermittedException();

            Expression = await _database.Expressions
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new MoveExpressionUpCommandModel()
            {
                Id = Expression.Id,
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
            if (!UserToken.CanMoveExpressionUp())
                throw new NotPermittedException();

            Expression = await _database.Expressions
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/expressionset/show/{Expression.ExpressionSetId}");
        }
        catch (Exception ex)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
