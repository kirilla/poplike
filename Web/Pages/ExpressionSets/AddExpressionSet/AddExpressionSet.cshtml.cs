using Poplike.Application.ExpressionSets.Commands.AddExpressionSet;

namespace Poplike.Web.Pages.ExpressionSets.AddExpressionSet;

public class AddExpressionSetModel : UserTokenPageModel
{
    private readonly IAddExpressionSetCommand _command;

    [BindProperty]
    public AddExpressionSetCommandModel CommandModel { get; set; }

    public AddExpressionSetModel(
        IUserToken userToken,
        IAddExpressionSetCommand command)
        : base(PageKind.AddExpressionSet, userToken)
    {
        _command = command;
    }

    public IActionResult OnGet()
    {
        try
        {
            if (!UserToken.CanAddExpressionSet())
                throw new NotPermittedException();

            CommandModel = new AddExpressionSetCommandModel();

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
            if (!UserToken.CanAddExpressionSet())
                throw new NotPermittedException();

            if (!ModelState.IsValid)
                return Page();

            var id = await _command.Execute(UserToken, CommandModel);

            return Redirect($"/expressionset/show/{id}");
        }
        catch (BlockedByExistingException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Name),
                "Det finns redan ett sådant uttryck.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
