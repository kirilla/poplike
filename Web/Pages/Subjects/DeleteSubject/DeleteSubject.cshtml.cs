using Poplike.Application.Subjects.Commands.DeleteSubject;

namespace Poplike.Web.Pages.Subjects.DeleteSubject;

public class DeleteSubjectModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IDeleteSubjectCommand _command;

    public Subject Subject { get; set; }

    [BindProperty]
    public DeleteSubjectCommandModel CommandModel { get; set; }

    public DeleteSubjectModel(
        IUserToken userToken,
        IDatabaseService database,
        IDeleteSubjectCommand command)
        :
        base(PageKind.DeleteSubject, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanDeleteSubject())
                throw new NotPermittedException();

            Subject = await _database.Subjects
                .Include(x => x.Category)
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new DeleteSubjectCommandModel()
            {
                Id = Subject.Id,
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
            if (!UserToken.CanDeleteSubject())
                throw new NotPermittedException();

            Subject = await _database.Subjects
                .Include(x => x.Category)
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect("/subject/all");
        }
        catch (ConfirmationRequiredException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Confirmed),
                "Bekräfta att du verkligen vill ta bort ämnet.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
