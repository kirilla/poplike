using Poplike.Application.Legal.Commands.RemoveWord;

namespace Poplike.Web.Pages.Legal.RemoveWord;

public class RemoveWordModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IRemoveWordCommand _command;

    public Word Word { get; set; }

    [BindProperty]
    public RemoveWordCommandModel CommandModel { get; set; }

    public RemoveWordModel(
        IUserToken userToken,
        IDatabaseService database,
        IRemoveWordCommand command)
        : base(PageKind.RemoveWord, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanRemoveWord())
                throw new NotPermittedException();

            Word = await _database.Words
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new RemoveWordCommandModel()
            {
                Id = Word.Id,
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
            if (!UserToken.CanRemoveWord())
                throw new NotPermittedException();

            Word = await _database.Words
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/legal/words");
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
