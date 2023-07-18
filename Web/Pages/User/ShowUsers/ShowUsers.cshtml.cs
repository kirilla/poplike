namespace Poplike.Web.Pages.User.ShowUsers;

public class ShowUsersModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    public List<Domain.User> Users { get; set; }

    public ShowUsersModel(
        IDatabaseService database,
        IUserToken userToken)
        :
        base(PageKind.PublicUsers, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            if (!UserToken.IsAuthenticated)
                return Redirect("help/signintodothis");

            Users = await _database.Users
                .AsNoTracking()
                .Where(x => x.IsHidden == false)
                .OrderByDescending(x => x.Name)
                .ToListAsync();

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
