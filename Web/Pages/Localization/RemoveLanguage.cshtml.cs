using Poplike.Application.Localization.Commands.RemoveLanguage;

namespace Poplike.Web.Pages.Localization;

public class RemoveLanguageModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IRemoveLanguageCommand _command;

    public Language Language { get; set; }

    [BindProperty]
    public RemoveLanguageCommandModel CommandModel { get; set; }

    public RemoveLanguageModel(
        IUserToken userToken,
        IDatabaseService database,
        IRemoveLanguageCommand command)
        : base(PageKind.RemoveLanguage, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanRemoveLanguage())
                throw new NotPermittedException();

            Language = await _database.Languages
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new RemoveLanguageCommandModel()
            {
                Id = Language.Id,
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
            if (!UserToken.CanRemoveLanguage())
                throw new NotPermittedException();

            Language = await _database.Languages
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/language/all");
        }
        catch (ConfirmationRequiredException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Confirmed),
                "Bekräfta att du verkligen vill ta bort språket.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
