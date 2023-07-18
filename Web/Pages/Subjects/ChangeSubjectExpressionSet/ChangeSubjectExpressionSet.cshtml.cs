using Poplike.Application.Subjects.Commands.ChangeSubjectExpressionSet;

namespace Poplike.Web.Pages.Subjects.ChangeSubjectExpressionSet;

public class ChangeSubjectExpressionSetModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IChangeSubjectExpressionSetCommand _command;

    public Subject Subject { get; set; }

    public List<ExpressionSet> ExpressionSets { get; set; }

    [BindProperty]
    public ChangeSubjectExpressionSetCommandModel CommandModel { get; set; }

    public ChangeSubjectExpressionSetModel(
        IUserToken userToken,
        IDatabaseService database,
        IChangeSubjectExpressionSetCommand command)
        :
        base(PageKind.ChangeSubjectExpressionSet, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanChangeSubjectExpressionSet())
                throw new NotPermittedException();

            Subject = await _database.Subjects
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            ExpressionSets = await _database.ExpressionSets
                .OrderBy(x => x.Name)
                .ToListAsync();

            CommandModel = new ChangeSubjectExpressionSetCommandModel()
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
            if (!UserToken.CanChangeSubjectExpressionSet())
                throw new NotPermittedException();

            Subject = await _database.Subjects
                .Where(x => x.Id == CommandModel.SubjectId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            ExpressionSets = await _database.ExpressionSets
                .OrderBy(x => x.Name)
                .ToListAsync();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/subject/curate/{CommandModel.SubjectId}");
        }
        catch (ConfirmationRequiredException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Confirmed),
                "Bekräfta att du vill ta byta uppsättning.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
