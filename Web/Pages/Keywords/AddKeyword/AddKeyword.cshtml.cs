using Poplike.Application.Keywords.Commands.AddKeyword;

namespace Poplike.Web.Pages.Keywords.AddKeyword;

public class AddKeywordModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IAddKeywordCommand _command;

    public Subject Subject { get; set; }

    [BindProperty]
    public AddKeywordCommandModel CommandModel { get; set; }

    public AddKeywordModel(
        IUserToken userToken,
        IDatabaseService database,
        IAddKeywordCommand command)
        : base(PageKind.AddKeyword, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanAddKeyword())
                throw new NotPermittedException();

            Subject = await _database.Subjects
                .Include(x => x.Category)
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new AddKeywordCommandModel()
            {
                SubjectId = Subject.Id,
            };

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
            if (!UserToken.CanAddKeyword())
                throw new NotPermittedException();

            Subject = await _database.Subjects
                .Where(x => x.Id == CommandModel.SubjectId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/subject/curate/{Subject.Id}");
        }
        catch (WordPreventedException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Word),
                "Texten innehåller ett blockerat ord eller symbol.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
