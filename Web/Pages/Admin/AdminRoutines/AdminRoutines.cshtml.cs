namespace Poplike.Web.Pages.Admin.AdminRoutines;

public class AdminRoutinesModel : UserTokenPageModel
{
    public AdminRoutinesModel(
        IUserToken userToken)
        :
        base(PageKind.AdminRoutines, userToken)
    {
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            if (!UserToken.IsAdmin)
                throw new NotPermittedException();

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
