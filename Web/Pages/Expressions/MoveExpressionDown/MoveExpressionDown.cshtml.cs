using Poplike.Application.Expressions.Commands.MoveExpressionDown;

namespace Poplike.Web.Pages.Expressions.MoveExpressionDown;

public class MoveExpressionDownModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IMoveExpressionDownCommand _command;

    public Expression Expression { get; set; }

    [BindProperty]
    public MoveExpressionDownCommandModel CommandModel { get; set; }

    public MoveExpressionDownModel(
        IUserToken userToken,
        IDatabaseService database,
        IMoveExpressionDownCommand command)
        : base(PageKind.MoveExpressionDown, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanMoveExpressionDown())
                throw new NotPermittedException();

            Expression = await _database.Expressions
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new MoveExpressionDownCommandModel()
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
            if (!UserToken.CanMoveExpressionDown())
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
