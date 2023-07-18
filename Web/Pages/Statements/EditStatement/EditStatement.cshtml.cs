using Poplike.Application.Statements.Commands.EditStatement;

namespace Poplike.Web.Pages.Statements.EditStatement;

public class EditStatementModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IEditStatementCommand _command;

    public Statement Statement { get; set; }
    public Subject Subject { get; set; }

    [BindProperty]
    public EditStatementCommandModel CommandModel { get; set; }

    public EditStatementModel(
        IUserToken userToken,
        IDatabaseService database,
        IEditStatementCommand command)
        : base(PageKind.EditStatement, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanEditStatement())
                throw new NotPermittedException();

            Statement = await _database.Statements
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Subject = await _database.Subjects
                .Where(x => x.Id == Statement.SubjectId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new EditStatementCommandModel()
            {
                Id = Statement.Id,
                Sentence = Statement.Sentence,
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
            if (!UserToken.CanEditStatement())
                throw new NotPermittedException();

            Statement = await _database.Statements
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Subject = await _database.Subjects
                .Where(x => x.Id == Statement.SubjectId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/subject/curate/{Subject.Id}");
        }
        catch (BlockedByExistingException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Sentence),
                "Det finns redan ett sådant uttryck.");

            return Page();
        }
        catch (WordPreventedException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Sentence),
                "Uttrycket innehåller ett blockerat ord eller symbol.");

            return Page();
        }
        catch (Exception ex)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
