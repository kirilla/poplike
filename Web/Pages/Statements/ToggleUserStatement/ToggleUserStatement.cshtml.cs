using Microsoft.AspNetCore.Authorization;
using Poplike.Application.Statements.Commands.ToggleUserStatement;

namespace Poplike.Web.Pages.Statements.ToggleUserStatement;

[AllowAnonymous]
public class ToggleUserStatementModel : UserTokenPageModel
{
    private readonly IToggleUserStatementCommand _command;

    [BindProperty]
    public ToggleUserStatementCommandModel CommandModel { get; set; }

    public ToggleUserStatementModel(
        IUserToken userToken,
        IToggleUserStatementCommand command)
        :
        base(PageKind.ToggleUserStatement, userToken)
    {
        _command = command;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!UserToken.CanToggleUserStatement())
                throw new NotPermittedException();

            if (!ModelState.IsValid)
            {
                return Redirect($"/subject/show/{CommandModel.SubjectId}");
            }

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/subject/show/{CommandModel.SubjectId}");
        }
        catch
        {
            return Redirect($"/subject/show/{CommandModel.SubjectId}");
        }
    }
}
