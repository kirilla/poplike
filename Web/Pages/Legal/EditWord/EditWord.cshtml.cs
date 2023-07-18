using Poplike.Application.Legal.Commands.EditWord;

namespace Poplike.Web.Pages.Legal.EditWord;

public class EditWordModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IEditWordCommand _command;

    public Word Word { get; set; }

    [BindProperty]
    public EditWordCommandModel CommandModel { get; set; }

    public EditWordModel(
        IUserToken userToken,
        IDatabaseService database,
        IEditWordCommand command)
        : base(PageKind.EditWord, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanEditWord())
                throw new NotPermittedException();

            Word = await _database.Words
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new EditWordCommandModel()
            {
                Id = Word.Id,
                Value = Word.Value,
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
            if (!UserToken.CanEditWord())
                throw new NotPermittedException();

            Word = await _database.Words
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect("/legal/words");
        }
        catch (BlockedByExistingException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Value),
                "Ordet finns redan.");

            return Page();
        }
        catch (Exception ex)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
