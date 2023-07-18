using Microsoft.AspNetCore.Authorization;
using Poplike.Application.Account.Commands.DoSignUp;
using Poplike.Common.Settings;

namespace Poplike.Web.Pages.Account.SignUp;

[AllowAnonymous]
public class SignUpModel : UserTokenPageModel
{
    private readonly IDoSignUpCommand _command;
    private readonly UserAccountConfiguration _config;

    [BindProperty]
    public DoSignUpCommandModel CommandModel { get; set; }

    public SignUpModel(
        IDoSignUpCommand command,
        IOptions<UserAccountConfiguration> userAccountOptions,
        IUserToken userToken)
        :
        base(PageKind.SignUp, userToken)
    {
        _command = command;
        _config = userAccountOptions.Value;
    }

    public IActionResult OnGet()
    {
        try
        {
            if (!UserToken.CanSignUp(_config))
                throw new NotPermittedException();

            return Page();
        }
        catch (FeatureTurnedOffException)
        {
            return Redirect("/help/featureturnedoff");
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
            if (!UserToken.CanSignUp(_config))
                throw new NotPermittedException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect("/account/signupsuccess");
        }
        catch (EmailAlreadyTakenException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Email),
                "Det finns redan ett konto med den här addressen.");

            return Page();
        }
        catch (FeatureTurnedOffException)
        {
            return Redirect("/help/featureturnedoff");
        }
        catch (Exception)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
