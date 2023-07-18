using Poplike.Application.Legal.Commands.AddWord;

namespace Poplike.Web.Pages.Legal.AddWord;

public class AddWordModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IAddWordCommand _command;

    [BindProperty]
    public AddWordCommandModel CommandModel { get; set; }

    public AddWordModel(
        IUserToken userToken,
        IDatabaseService database,
        IAddWordCommand command)
        : base(PageKind.AddWord, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            if (!UserToken.CanAddWord())
                throw new NotPermittedException();

            CommandModel = new AddWordCommandModel();

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
            if (!UserToken.CanAddWord())
                throw new NotPermittedException();

            if (!ModelState.IsValid)
                return Page();

            var id = await _command.Execute(UserToken, CommandModel);

            return Redirect($"/legal/words");
        }
        catch (BlockedByExistingException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Value),
                "Ordet finns redan.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
