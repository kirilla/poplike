using Poplike.Application.Account.Commands.DeleteAccount;

namespace Poplike.Web.Pages.Account.DeleteAccount;

public class DeleteAccountModel : UserTokenPageModel
{
    private readonly IDeleteAccountCommand _command;

    [BindProperty]
    public DeleteAccountCommandModel CommandModel { get; set; }

    public DeleteAccountModel(
        IUserToken userToken,
        IDeleteAccountCommand command)
        :
        base(PageKind.DeleteAccount, userToken)
    {
        _command = command;
    }

    public IActionResult OnGet()
    {
        try
        {
            if (!UserToken.CanDeleteAccount())
                throw new NotPermittedException();

            CommandModel = new DeleteAccountCommandModel();

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
            if (!UserToken.CanDeleteAccount())
                throw new NotPermittedException();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _command.Execute(UserToken, CommandModel);

            return Redirect("/account/farewell");
        }
        catch (ConfirmationRequiredException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Confirmed),
                "Bekräfta att du verkligen vill ta bort ditt konto.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
