namespace Poplike.Web.Pages.Settings;

public class SettingsModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    public SettingsModel(
        IUserToken userToken,
        IDatabaseService database)
        :
        base(PageKind.Settings, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            if (!UserToken.IsAdmin &&
                !UserToken.IsModerator &&
                !UserToken.IsCurator)
                throw new NotPermittedException();

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
