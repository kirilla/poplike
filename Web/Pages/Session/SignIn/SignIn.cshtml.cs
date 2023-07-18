using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Poplike.Application.Sessions.Commands.SignIn;
using Poplike.Common.Settings;

namespace Poplike.Web.Pages.Session.SignIn;

[AllowAnonymous]
public class SignInModel : UserTokenPageModel
{
    private readonly ISignInCommand _signInCommand;
    private readonly ILogger<SignInModel> _logger;
    private readonly UserAccountConfiguration _config;

    [BindProperty]
    public SignInCommandModel CommandModel { get; set; }

    public SignInModel(
        ISignInCommand signInCommand,
        ILogger<SignInModel> logger,
        IOptions<UserAccountConfiguration> userAccountOptions,
        IUserToken userToken)
        :
        base(PageKind.SignIn, userToken)
    {
        _signInCommand = signInCommand;
        _logger = logger;
        _config = userAccountOptions.Value;
    }

    public IActionResult OnGet()
    {
        try
        {
            if (!UserToken.CanSignIn(_config))
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

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        try
        {
            if (!UserToken.CanSignIn(_config))
                throw new NotPermittedException();

            if (!ModelState.IsValid)
                return Page();

            var loginResult = await _signInCommand.Execute(UserToken, CommandModel);

            var claims = new List<Claim>
            {
                new Claim("UserGuid", loginResult.UserGuid.ToString()),
                new Claim("SessionGuid", loginResult.SessionGuid.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            returnUrl ??= "/Session/SignInSuccess";

            return LocalRedirect(returnUrl);
        }
        catch (FeatureTurnedOffException)
        {
            return Redirect("/help/featureturnedoff");
        }
        catch (UserNotFoundException)
        {
            _logger.LogError(
                "User not found: {Email}.",
                    CommandModel.Email);

            ModelState.AddModelError(
                nameof(CommandModel.Email), "Okänd användare.");

            return Page();
        }
        catch (PasswordVerificationFailedException)
        {
            _logger.LogError(
                "Password verification failed for user: {Email}.",
                    CommandModel.Email);

            ModelState.AddModelError(
                nameof(CommandModel.Email), "Felaktigt lösenord.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
