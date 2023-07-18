using Poplike.Application.Localization.Commands.AddLanguage;

namespace Poplike.Web.Pages.Localization;

public class AddLanguageModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IAddLanguageCommand _command;

    [BindProperty]
    public AddLanguageCommandModel CommandModel { get; set; }

    public AddLanguageModel(
        IUserToken userToken,
        IDatabaseService database,
        IAddLanguageCommand command)
        : base(PageKind.AddLanguage, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            if (!UserToken.CanAddLanguage())
                throw new NotPermittedException();

            CommandModel = new AddLanguageCommandModel();

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
            if (!UserToken.CanAddLanguage())
                throw new NotPermittedException();

            if (!ModelState.IsValid)
                return Page();

            var id = await _command.Execute(UserToken, CommandModel);

            return Redirect("/language/all");
        }
        catch (BlockedByExistingException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Name),
                "Språket finns redan.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
