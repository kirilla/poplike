using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.User.ShowCurators;

[AllowAnonymous]
public class ShowCuratorsModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    public List<Domain.User> Users { get; set; }

    public ShowCuratorsModel(
        IDatabaseService database,
        IUserToken userToken)
        :
        base(PageKind.ModeratorUsers, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            Users = await _database.Users
                .AsNoTracking()
                .Where(x => x.IsCurator == true)
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
