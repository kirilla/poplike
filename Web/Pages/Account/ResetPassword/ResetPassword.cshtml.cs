using Microsoft.AspNetCore.Authorization;
using Poplike.Application.Account.Commands.ResetPassword;
using Poplike.Common.Settings;

namespace Poplike.Web.Pages.Account.ResetPassword;

[AllowAnonymous]
public class ResetPasswordModel : UserTokenPageModel
{
    private readonly IResetPasswordCommand _command;
    private readonly ILogger<ResetPasswordModel> _logger;
    private readonly UserAccountConfiguration _config;

    [BindProperty]
    public ResetPasswordCommandModel CommandModel { get; set; }

    public ResetPasswordModel(
        IUserToken userToken,
        IResetPasswordCommand command,
        ILogger<ResetPasswordModel> logger,
        IOptions<UserAccountConfiguration> userAccountOptions)
        :
        base(PageKind.ResetPassword, userToken)
    {
        _command = command;
        _logger = logger;
        _config = userAccountOptions.Value;
    }

    public IActionResult OnGet(Guid guid)
    {
        try
        {
            if (!UserToken.CanResetPassword(_config))
                throw new NotPermittedException();

            CommandModel = new ResetPasswordCommandModel()
            {
                Guid = guid,
            };

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
            if (!UserToken.CanResetPassword(_config))
                throw new NotPermittedException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect("/account/resetpasswordsuccess");
        }
        catch (FeatureTurnedOffException)
        {
            return Redirect("/help/featureturnedoff");
        }
        catch (NotFoundException)
        {
            _logger.LogInformation("Hittar inte begäran om återställning av lösenord.");

            ModelState.AddModelError(nameof(CommandModel.Email),
                "Hittar inte begäran om återställning av lösenord.");

            return Page();
        }
        catch (InvalidDataException)
        {
            _logger.LogInformation("Okänt fel i sidan för lösenordsåterställning.");

            ModelState.AddModelError(nameof(CommandModel.Email), "Okänt fel.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
