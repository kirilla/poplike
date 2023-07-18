using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.User.ShowUser;

[AllowAnonymous]
public class ShowUserModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    public new Domain.User? User { get; set; }

    public List<UserStatement> UserStatements { get; set; }

    public ShowUserModel(
        IUserToken userToken,
        IDatabaseService database)
        :
        base(PageKind.ShowUser, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.IsAuthenticated)
                return Redirect("/help/signintoseeuser");

            User = await _database.Users
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            if (User.IsHidden &&
                !UserToken.IsAdmin &&
                !UserToken.IsModerator)
                throw new NotFoundException();

            UserStatements = await _database.UserStatements
                .Include(x => x.Statement.Subject)
                .Where(x => x.UserId == id)
                .ToListAsync();

            return Page();
        }
        catch (NotFoundException)
        {
            return Redirect("/help/notfound");
        }
        catch (Exception)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
