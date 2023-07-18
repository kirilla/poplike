using Poplike.Application.Blurbs.Commands.EditSubjectBlurb;

namespace Poplike.Web.Pages.Blurbs.EditSubjectBlurb;

public class EditSubjectBlurbModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IEditSubjectBlurbCommand _command;

    public SubjectBlurb SubjectBlurb { get; set; }
    public Subject Subject { get; set; }

    [BindProperty]
    public EditSubjectBlurbCommandModel CommandModel { get; set; }

    public EditSubjectBlurbModel(
        IUserToken userToken,
        IDatabaseService database,
        IEditSubjectBlurbCommand command)
        : base(PageKind.EditSubjectBlurb, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanEditSubjectBlurb())
                throw new NotPermittedException();

            SubjectBlurb = await _database.SubjectBlurbs
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Subject = await _database.Subjects
                .Where(x => x.Id == SubjectBlurb.SubjectId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new EditSubjectBlurbCommandModel()
            {
                Id = SubjectBlurb.Id,
                Text = SubjectBlurb.Text,
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
            if (!UserToken.CanEditSubjectBlurb())
                throw new NotPermittedException();

            SubjectBlurb = await _database.SubjectBlurbs
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Subject = await _database.Subjects
                .Where(x => x.Id == SubjectBlurb.SubjectId)
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
        catch (Exception ex)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
