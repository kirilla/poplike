namespace Poplike.Web.Pages.Account.AccountStatements;

public class AccountStatementsModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    public List<UserStatement> UserStatements { get; set; }

    public AccountStatementsModel(
        IUserToken userToken,
        IDatabaseService database)
        :
        base(PageKind.AccountStatements, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            if (!UserToken.IsAuthenticated)
                throw new NotPermittedException();

            var user = await _database.Users
                .AsNoTracking()
                .Where(x => x.Id == UserToken.UserId!.Value)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            UserStatements = await _database.UserStatements
                .Include(x => x.Statement.Subject)
                .Where(x => x.UserId == UserToken.UserId!.Value)
                .OrderBy(x => x.Statement.Sentence)
            .ToListAsync();

            return Page();
        }
        catch (Exception ex)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
