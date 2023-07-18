using Poplike.Application.Localization.Commands.EditLanguage;

namespace Poplike.Web.Pages.Localization;

public class EditLanguageModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IEditLanguageCommand _command;

    public Language Language { get; set; }

    [BindProperty]
    public EditLanguageCommandModel CommandModel { get; set; }

    public EditLanguageModel(
        IUserToken userToken,
        IDatabaseService database,
        IEditLanguageCommand command)
        : base(PageKind.EditLanguage, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanEditLanguage())
                throw new NotPermittedException();

            Language = await _database.Languages
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new EditLanguageCommandModel()
            {
                Id = Language.Id,
                Name = Language.Name,
                Culture = Language.Culture,
                Emoji = Language.Emoji,
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
            if (!UserToken.CanEditLanguage())
                throw new NotPermittedException();

            Language = await _database.Languages
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/language/show/{Language.Id}");
        }
        catch (BlockedByExistingException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Name),
                "Språket finns redan.");

            return Page();
        }
        catch (Exception ex)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
