using Microsoft.AspNetCore.Authorization;
using Poplike.Application.Statements.Commands.AddUserStatement;

namespace Poplike.Web.Pages.Statements.AddUserStatement;

[AllowAnonymous]
public class AddUserStatementModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IAddUserStatementCommand _command;

    public Subject Subject { get; set; }

    [BindProperty]
    public AddUserStatementCommandModel CommandModel { get; set; }

    public AddUserStatementModel(
        IUserToken userToken,
        IDatabaseService database,
        IAddUserStatementCommand command)
        :
        base(PageKind.AddUserStatement, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanAddUserStatement())
                throw new SignIntoDoThisException();

            Subject = await _database.Subjects
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new AddUserStatementCommandModel()
            {
                SubjectId = Subject.Id,
            };

            return Page();
        }
        catch (NotFoundException)
        {
            return Redirect("/help/notfound");
        }
        catch (SignIntoDoThisException)
        {
            return Redirect("/help/signintodothis");
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
            if (!UserToken.CanAddUserStatement())
                throw new SignIntoDoThisException();

            Subject = await _database.Subjects
                .Where(x => x.Id == CommandModel.SubjectId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/subject/show/{CommandModel.SubjectId}");
        }
        catch (BlockedByExistingException)
        {
            return Redirect($"/subject/show/{CommandModel.SubjectId}");
        }
        catch (SignIntoDoThisException)
        {
            return Redirect("/help/signintodothis");
        }
        catch (Exception ex)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
