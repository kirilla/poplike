using Poplike.Application.Expressions.Commands.RemoveExpression;

namespace Poplike.Web.Pages.Expressions.RemoveExpression;

public class RemoveExpressionModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IRemoveExpressionCommand _command;

    public Expression Expression { get; set; }
    public ExpressionSet ExpressionSet { get; set; }

    [BindProperty]
    public RemoveExpressionCommandModel CommandModel { get; set; }

    public RemoveExpressionModel(
        IUserToken userToken,
        IDatabaseService database,
        IRemoveExpressionCommand command)
        : base(PageKind.RemoveExpression, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanRemoveExpression())
                throw new NotPermittedException();

            Expression = await _database.Expressions
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            ExpressionSet = await _database.ExpressionSets
                .Where(x => x.Id == Expression.ExpressionSetId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new RemoveExpressionCommandModel()
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
            if (!UserToken.CanRemoveExpression())
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
