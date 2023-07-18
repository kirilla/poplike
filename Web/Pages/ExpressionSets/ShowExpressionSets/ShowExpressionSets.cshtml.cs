namespace Poplike.Web.Pages.ExpressionSets.ShowExpressionSets;

public class ShowExpressionSetsModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    public List<ExpressionSet> ExpressionSets { get; set; }

    public ShowExpressionSetsModel(
        IUserToken userToken,
        IDatabaseService database)
        : base(PageKind.ShowExpressionSets, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGet()
    {
        try
        {
            if (!UserToken.IsAdmin &&
                !UserToken.IsModerator &&
                !UserToken.IsCurator)
                throw new NotPermittedException();

            ExpressionSets = await _database.ExpressionSets
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
