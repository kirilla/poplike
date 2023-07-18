using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.User.ShowAdmins;

[AllowAnonymous]
public class ShowAdminsModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    public List<Domain.User> Users { get; set; }

    public ShowAdminsModel(
        IDatabaseService database,
        IUserToken userToken)
        :
        base(PageKind.AdminUsers, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            Users = await _database.Users
                .AsNoTracking()
                .Where(x => x.IsAdmin == true)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
