using Poplike.Application.Subjects.Commands.DeleteSubjectReactions;

namespace Poplike.Web.Pages.Subjects.DeleteSubjectReactions;

public class DeleteSubjectReactionsModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IDeleteSubjectReactionsCommand _command;

    public Subject Subject { get; set; }

    [BindProperty]
    public DeleteSubjectReactionsCommandModel CommandModel { get; set; }

    public DeleteSubjectReactionsModel(
        IUserToken userToken,
        IDatabaseService database,
        IDeleteSubjectReactionsCommand command)
        :
        base(PageKind.DeleteSubjectReactions, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanDeleteSubjectReactions())
                throw new NotPermittedException();

            Subject = await _database.Subjects
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new DeleteSubjectReactionsCommandModel()
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
            if (!UserToken.CanDeleteSubjectReactions())
                throw new NotPermittedException();

            Subject = await _database.Subjects
                .Where(x => x.Id == CommandModel.SubjectId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/subject/curate/{CommandModel.SubjectId}");
        }
        catch (ConfirmationRequiredException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Confirmed),
                "Bekräfta att du vill ta bort alla svar.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
