using Poplike.Application.Statements.Commands.DeleteUserStatement;

namespace Poplike.Web.Pages.Statements.DeleteUserStatement;

public class DeleteUserStatementModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IDeleteUserStatementCommand _command;

    [BindProperty]
    public DeleteUserStatementCommandModel CommandModel { get; set; }

    public DeleteUserStatementModel(
        IUserToken userToken,
        IDatabaseService database,
        IDeleteUserStatementCommand command)
        :
        base(PageKind.DeleteUserStatement, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanDeleteUserStatement())
                throw new NotPermittedException();

            var statement = await _database.UserStatements
                .Include(x => x.Statement.Subject)
                .Where(x =>
                    x.Id == id &&
                    x.UserId == UserToken.UserId!.Value)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new DeleteUserStatementCommandModel()
            {
                UserStatementId = statement.Id,
                SubjectName = statement.Statement.Subject.Name,
                Sentence = statement.Statement.Sentence,
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
            if (!UserToken.CanDeleteUserStatement())
                throw new NotPermittedException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect("/account/statements");
        }
        catch (NotFoundException)
        {
            return Redirect("/help/notfound");
        }
        catch (Exception ex)
        {
            return Redirect("/account/statements");
        }
    }
}
