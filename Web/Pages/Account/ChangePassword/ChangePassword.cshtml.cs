using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using Poplike.Application.Account.Commands.ChangePassword;

namespace Poplike.Web.Pages.Account.ChangePassword;

public class ChangePasswordModel : UserTokenPageModel
{
    private readonly IChangePasswordCommand _command;

    [BindProperty]
    public ChangePasswordCommandModel CommandModel { get; set; }

    public ChangePasswordModel(
        IUserToken userToken,
        IChangePasswordCommand command)
        :
        base(PageKind.ChangePassword, userToken)
    {
        _command = command;
    }

    public IActionResult OnGet()
    {
        try
        {
            if (!UserToken.CanChangePassword())
                throw new NotPermittedException();

            CommandModel = new ChangePasswordCommandModel();

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
            if (!UserToken.CanChangePassword())
                throw new NotPermittedException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect("/help/success");
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
