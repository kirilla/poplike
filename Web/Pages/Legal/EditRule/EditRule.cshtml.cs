using Poplike.Application.Legal.Commands.EditRule;

namespace Poplike.Web.Pages.Legal.EditRule;

public class EditRuleModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IEditRuleCommand _command;

    public Rule Rule { get; set; }

    [BindProperty]
    public EditRuleCommandModel CommandModel { get; set; }

    public EditRuleModel(
        IUserToken userToken,
        IDatabaseService database,
        IEditRuleCommand command)
        : base(PageKind.EditRule, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanEditRule())
                throw new NotPermittedException();

            Rule = await _database.Rules
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new EditRuleCommandModel()
            {
                Id = Rule.Id,
                Number = Rule.Number,
                Heading = Rule.Heading,
                Text = Rule.Text,
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
            if (!UserToken.CanEditRule())
                throw new NotPermittedException();

            Rule = await _database.Rules
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect("/legal/rules");
        }
        catch (BlockedByExistingException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Heading),
                "Ordet finns redan.");

            return Page();
        }
        catch (Exception ex)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
