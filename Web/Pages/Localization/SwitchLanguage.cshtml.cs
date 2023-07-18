using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Localization;

[AllowAnonymous]
public class SwitchLanguageModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    public List<Language> Languages { get; set; }

    [BindProperty]
    public string? SelectedCulture { get; set; } = string.Empty;

    public SwitchLanguageModel(
        IUserToken userToken,
        IDatabaseService database)
        :
        base(PageKind.SwitchLanguage, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            Languages = await _database.Languages.ToListAsync();

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
            if (string.IsNullOrWhiteSpace(SelectedCulture))
            {
                Response.Cookies.Delete(CookieRequestCultureProvider.DefaultCookieName);

                return Redirect("/language");
            }

            new CultureInfo(SelectedCulture); // throws if not valid

            Languages = await _database.Languages.ToListAsync();

            if (!Languages.Any(x => x.Culture == SelectedCulture!.Trim().ToLowerInvariant()))
            {
                return Page();
            }

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(SelectedCulture)),
                new CookieOptions
                {
                    MaxAge = TimeSpan.FromDays(365),
                    IsEssential = true,
                });

            return Redirect("/language");
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
