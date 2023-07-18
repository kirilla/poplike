using Microsoft.AspNetCore.Authorization;
using Poplike.Common.Dates;

namespace Poplike.Web.Pages.User.UserMenu;

[AllowAnonymous]
public class UserMenuModel : UserTokenPageModel
{
    public int UserCount { get; set; }

    private readonly IDatabaseService _database;
    private readonly IDateService _dateService;

    public UserMenuModel(
        IDatabaseService database,
        IDateService dateService,
        IUserToken userToken)
        :
        base(PageKind.ShowAllUsers, userToken)
    {
        _database = database;
        _dateService = dateService;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            UserCount = await _database.Users.CountAsync();

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
