using Poplike.Application.Blurbs.Commands.RemoveSubjectBlurb;

namespace Poplike.Web.Pages.Blurbs.RemoveSubjectBlurb;

public class RemoveSubjectBlurbModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IRemoveSubjectBlurbCommand _command;

    public SubjectBlurb SubjectBlurb { get; set; }
    public Subject Subject { get; set; }

    [BindProperty]
    public RemoveSubjectBlurbCommandModel CommandModel { get; set; }

    public RemoveSubjectBlurbModel(
        IUserToken userToken,
        IDatabaseService database,
        IRemoveSubjectBlurbCommand command)
        : base(PageKind.RemoveSubjectBlurb, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanRemoveSubjectBlurb())
                throw new NotPermittedException();

            SubjectBlurb = await _database.SubjectBlurbs
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Subject = await _database.Subjects
                .Where(x => x.Id == SubjectBlurb.SubjectId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new RemoveSubjectBlurbCommandModel()
            {
                Id = SubjectBlurb.Id,
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
            if (!UserToken.CanRemoveSubjectBlurb())
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
        catch (ConfirmationRequiredException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Confirmed),
                "Bekräfta att du verkligen vill ta bort uttrycket.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
