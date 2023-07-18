using Poplike.Application.ExpressionSets.Commands.EditExpressionSet;

namespace Poplike.Web.Pages.ExpressionSets.EditExpressionSet;

public class EditExpressionSetModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IEditExpressionSetCommand _command;

    public ExpressionSet ExpressionSet { get; set; }

    [BindProperty]
    public EditExpressionSetCommandModel CommandModel { get; set; }

    public EditExpressionSetModel(
        IUserToken userToken,
        IDatabaseService database,
        IEditExpressionSetCommand command)
        : base(PageKind.EditExpressionSet, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanEditExpressionSet())
                throw new NotPermittedException();

            ExpressionSet = await _database.ExpressionSets
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new EditExpressionSetCommandModel()
            {
                Id = ExpressionSet.Id,
                Emoji = ExpressionSet.Emoji,
                Name = ExpressionSet.Name,
                MultipleChoice = ExpressionSet.MultipleChoice,
                FreeExpression = ExpressionSet.FreeExpression,
            };

            return Page();
        }
        catch (BlockedByExistingException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Name),
                "Det finns redan en sådan grupp.");

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
            if (!UserToken.CanEditExpressionSet())
                throw new NotPermittedException();

            ExpressionSet = await _database.ExpressionSets
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/expressionset/show/{ExpressionSet.Id}");
        }
        catch (Exception ex)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
