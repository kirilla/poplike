using Poplike.Application.Admin.Commands.CreateDefaultExpressions;

namespace Poplike.Web.Pages.Admin.CreateDefaultExpressions;

public class CreateDefaultExpressionsModel : UserTokenPageModel
{
    private readonly ICreateDefaultExpressionsCommand _command;

    [BindProperty]
    public CreateDefaultExpressionsCommandModel CommandModel { get; set; }

    public CreateDefaultExpressionsModel(
        IUserToken userToken,
        ICreateDefaultExpressionsCommand command)
        :
        base(PageKind.CreateDefaultExpressions, userToken)
    {
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            if (!UserToken.CanCreateDefaultExpressions())
                throw new NotPermittedException();

            CommandModel = new CreateDefaultExpressionsCommandModel();

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
            if (!UserToken.CanCreateDefaultExpressions())
                throw new NotPermittedException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect("/expressionset/all");
        }
        catch (ConfirmationRequiredException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Confirmed),
                "Bekräfta att du vill köra rutinen.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
