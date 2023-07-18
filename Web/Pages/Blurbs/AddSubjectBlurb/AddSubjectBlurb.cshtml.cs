using Poplike.Application.Blurbs.Commands.AddSubjectBlurb;

namespace Poplike.Web.Pages.Blurbs.AddSubjectBlurb;

public class AddSubjectBlurbModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IAddSubjectBlurbCommand _command;

    public Subject Subject { get; set; }

    [BindProperty]
    public AddSubjectBlurbCommandModel CommandModel { get; set; }

    public AddSubjectBlurbModel(
        IUserToken userToken,
        IDatabaseService database,
        IAddSubjectBlurbCommand command)
        : base(PageKind.AddSubjectBlurb, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanAddSubjectBlurb())
                throw new NotPermittedException();

            Subject = await _database.Subjects
                .Include(x => x.Category)
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new AddSubjectBlurbCommandModel()
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
            if (!UserToken.CanAddSubjectBlurb())
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
                nameof(CommandModel.Text),
                "Texten innehåller ett blockerat ord eller symbol.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
