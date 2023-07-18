using Poplike.Application.Expressions.Commands.AddExpression;

namespace Poplike.Web.Pages.Expressions.AddExpression;

public class AddExpressionModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IAddExpressionCommand _command;

    public ExpressionSet ExpressionSet { get; set; }

    [BindProperty]
    public AddExpressionCommandModel CommandModel { get; set; }

    public AddExpressionModel(
        IUserToken userToken,
        IDatabaseService database,
        IAddExpressionCommand command)
        : base(PageKind.AddExpression, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanAddExpression())
                throw new NotPermittedException();

            ExpressionSet = await _database.ExpressionSets
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new AddExpressionCommandModel()
            {
                ExpressionSetId = ExpressionSet.Id,
                GroupName = ExpressionSet.Name,
                GroupEmoji = ExpressionSet.Emoji,
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
            if (!UserToken.CanAddExpression())
                throw new NotPermittedException();

            ExpressionSet = await _database.ExpressionSets
                .Where(x => x.Id == CommandModel.ExpressionSetId)
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
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
