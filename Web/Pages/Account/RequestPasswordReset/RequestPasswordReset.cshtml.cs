using Microsoft.AspNetCore.Authorization;
using Poplike.Application.Account.Commands.RequestPasswordReset;
using Poplike.Common.Settings;

namespace Poplike.Web.Pages.Account.RequestPasswordReset;

[AllowAnonymous]
public class RequestPasswordResetModel : UserTokenPageModel
{
    private readonly IRequestPasswordResetCommand _command;
    private readonly ILogger<RequestPasswordResetModel> _logger;
    private readonly UserAccountConfiguration _config;

    [BindProperty]
    public RequestPasswordResetCommandModel CommandModel { get; set; }

    public RequestPasswordResetModel(
        IUserToken userToken,
        IRequestPasswordResetCommand command,
        ILogger<RequestPasswordResetModel> logger,
        IOptions<UserAccountConfiguration> userAccountOptions)
        :
        base(PageKind.RequestPasswordReset, userToken)
    {
        _command = command;
        _logger = logger;
        _config = userAccountOptions.Value;
    }

    public IActionResult OnGet()
    {
        try
        {
            if (!UserToken.CanRequestPasswordReset(_config))
                throw new NotPermittedException();

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
            if (!UserToken.CanRequestPasswordReset(_config))
                throw new NotPermittedException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect("/account/requestpasswordresetsuccess");
        }
        catch (NotFoundException)
        {
            _logger.LogInformation("Fel i sidan för lösenordsåterställning. Okänd användare.");

            ModelState.AddModelError(nameof(CommandModel.Email), "Okänd användare.");

            return Page();
        }
        catch (PreexistingPasswordResetRequestException)
        {
            _logger.LogInformation("Lösenordsåterställning finns redan.");

            ModelState.AddModelError(nameof(CommandModel.Email),
                "Det finns redan en lösenordsåterställning för adressen.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
