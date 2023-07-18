using Poplike.Application.Legal.Commands.RemoveRule;

namespace Poplike.Web.Pages.Legal.RemoveRule;

public class RemoveRuleModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IRemoveRuleCommand _command;

    public Rule Rule { get; set; }

    [BindProperty]
    public RemoveRuleCommandModel CommandModel { get; set; }

    public RemoveRuleModel(
        IUserToken userToken,
        IDatabaseService database,
        IRemoveRuleCommand command)
        : base(PageKind.RemoveRule, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanRemoveRule())
                throw new NotPermittedException();

            Rule = await _database.Rules
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new RemoveRuleCommandModel()
            {
                Id = Rule.Id,
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
            if (!UserToken.CanRemoveRule())
                throw new NotPermittedException();

            Rule = await _database.Rules
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/legal/rules");
        }
        catch (ConfirmationRequiredException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Confirmed),
                "Bekräfta att du verkligen vill ta bort ordet.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
