using Poplike.Application.Legal.Commands.AddRule;

namespace Poplike.Web.Pages.Legal.AddRule;

public class AddRuleModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IAddRuleCommand _command;

    [BindProperty]
    public AddRuleCommandModel CommandModel { get; set; }

    public AddRuleModel(
        IUserToken userToken,
        IDatabaseService database,
        IAddRuleCommand command)
        : base(PageKind.AddRule, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            if (!UserToken.CanAddRule())
                throw new NotPermittedException();

            CommandModel = new AddRuleCommandModel();

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
            if (!UserToken.CanAddRule())
                throw new NotPermittedException();

            if (!ModelState.IsValid)
                return Page();

            var id = await _command.Execute(UserToken, CommandModel);

            return Redirect($"/legal/rules");
        }
        catch (BlockedByExistingException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Heading),
                "Ordet finns redan.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
