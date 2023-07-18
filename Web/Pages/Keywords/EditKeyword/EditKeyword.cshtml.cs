using Poplike.Application.Keywords.Commands.EditKeyword;

namespace Poplike.Web.Pages.Keywords.EditKeyword;

public class EditKeywordModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IEditKeywordCommand _command;

    public Keyword Keyword { get; set; }
    public Subject Subject { get; set; }

    [BindProperty]
    public EditKeywordCommandModel CommandModel { get; set; }

    public EditKeywordModel(
        IUserToken userToken,
        IDatabaseService database,
        IEditKeywordCommand command)
        : base(PageKind.EditKeyword, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanEditKeyword())
                throw new NotPermittedException();

            Keyword = await _database.Keywords
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Subject = await _database.Subjects
                .Where(x => x.Id == Keyword.SubjectId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new EditKeywordCommandModel()
            {
                Id = Keyword.Id,
                Word = Keyword.Word,
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
            if (!UserToken.CanEditKeyword())
                throw new NotPermittedException();

            Keyword = await _database.Keywords
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Subject = await _database.Subjects
                .Where(x => x.Id == Keyword.SubjectId)
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
        catch (Exception ex)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
