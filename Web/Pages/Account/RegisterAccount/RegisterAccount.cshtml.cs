using Microsoft.AspNetCore.Authorization;
using Poplike.Application.Account.Commands.RegisterAccount;
using Poplike.Common.Settings;

namespace Poplike.Web.Pages.Account.RegisterAccount;

[AllowAnonymous]
public class RegisterAccountModel : UserTokenPageModel
{
    private readonly IRegisterAccountCommand _command;
    private readonly UserAccountConfiguration _config;

    [BindProperty]
    public RegisterAccountCommandModel CommandModel { get; set; }

    public RegisterAccountModel(
        IUserToken userToken,
        IRegisterAccountCommand command,
        IOptions<UserAccountConfiguration> options)
        :
        base(PageKind.RegisterAccount, userToken)
    {
        _command = command;
        _config = options.Value;
    }

    public IActionResult OnGet()
    {
        try
        {
            if (!UserToken.CanRegisterAccount(_config))
                throw new NotPermittedException();

            CommandModel = new RegisterAccountCommandModel()
            {
                IsHidden = true,
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
            if (!UserToken.CanRegisterAccount(_config))
                throw new NotPermittedException();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _command.Execute(UserToken, CommandModel);

            return Redirect("/account/registeraccountsuccess");
        }
        catch (EmailAlreadyTakenException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Email),
                "Epostadressen används redan av en annan användare.");

            return Page();
        }
        catch (NameAlreadyTakenException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Name),
                "Namnet används redan av en annan användare.");

            return Page();
        }
        catch (WordPreventedException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Name),
                "Namnet innehåller ett blockerat ord.");

            return Page();
        }
        catch (Exception)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
