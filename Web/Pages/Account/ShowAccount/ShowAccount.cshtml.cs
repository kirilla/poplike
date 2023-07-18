namespace Poplike.Web.Pages.Account.ShowAccount;

public class ShowAccountModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    public new Domain.User User { get; set; }

    public List<Domain.Session> Sessions { get; set; }
    public List<PasswordResetRequest> PasswordResetRequests { get; set; }

    public ShowAccountModel(
        IDatabaseService database,
        IUserToken userToken)
        :
        base(PageKind.ShowAccount, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            if (!UserToken.IsAuthenticated)
                throw new NotPermittedException();

            User = await _database.Users
                .AsNoTracking()
                .Where(x => x.Id == UserToken.UserId!.Value)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Sessions = await _database.Sessions
                .AsNoTracking()
                .Where(x => x.UserId == UserToken.UserId!.Value)
                .ToListAsync();

            PasswordResetRequests = await _database.PasswordResetRequests
                .AsNoTracking()
                .Where(x => x.UserId == UserToken.UserId!.Value)
                .ToListAsync();

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
