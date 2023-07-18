using Poplike.Application.Keywords.Commands.RemoveKeyword;

namespace Poplike.Web.Pages.Keywords.RemoveKeyword;

public class RemoveKeywordModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IRemoveKeywordCommand _command;

    public Keyword Keyword { get; set; }
    public Subject Subject { get; set; }

    [BindProperty]
    public RemoveKeywordCommandModel CommandModel { get; set; }

    public RemoveKeywordModel(
        IUserToken userToken,
        IDatabaseService database,
        IRemoveKeywordCommand command)
        : base(PageKind.RemoveKeyword, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanRemoveKeyword())
                throw new NotPermittedException();

            Keyword = await _database.Keywords
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Subject = await _database.Subjects
                .Where(x => x.Id == Keyword.SubjectId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new RemoveKeywordCommandModel()
            {
                Id = Keyword.Id,
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
            if (!UserToken.CanRemoveKeyword())
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
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
