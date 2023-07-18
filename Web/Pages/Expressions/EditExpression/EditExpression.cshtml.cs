using Poplike.Application.Expressions.Commands.EditExpression;

namespace Poplike.Web.Pages.Expressions.EditExpression;

public class EditExpressionModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IEditExpressionCommand _command;

    public Expression Expression { get; set; }
    public ExpressionSet ExpressionSet { get; set; }

    [BindProperty]
    public EditExpressionCommandModel CommandModel { get; set; }

    public EditExpressionModel(
        IUserToken userToken,
        IDatabaseService database,
        IEditExpressionCommand command)
        : base(PageKind.EditExpression, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanEditExpression())
                throw new NotPermittedException();

            Expression = await _database.Expressions
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            ExpressionSet = await _database.ExpressionSets
                .Where(x => x.Id == Expression.ExpressionSetId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new EditExpressionCommandModel()
            {
                Id = Expression.Id,
                Characters = Expression.Characters,
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
            if (!UserToken.CanEditExpression())
                throw new NotPermittedException();

            Expression = await _database.Expressions
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            ExpressionSet = await _database.ExpressionSets
                .Where(x => x.Id == Expression.ExpressionSetId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/expressionset/show/{ExpressionSet.Id}");
        }
        catch (BlockedByExistingException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Characters),
                "Det finns redan ett sådant uttryck.");

            return Page();
        }
        catch (WordPreventedException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Characters),
                "Uttrycket innehåller ett blockerat ord eller symbol.");

            return Page();
        }
        catch (Exception ex)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
