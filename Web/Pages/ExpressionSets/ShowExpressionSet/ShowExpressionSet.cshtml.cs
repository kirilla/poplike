namespace Poplike.Web.Pages.ExpressionSets.ShowExpressionSet;

public class ShowExpressionSetModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    public ExpressionSet ExpressionSet { get; set; }

    public List<Category> Categories { get; set; }
    public List<Expression> Expressions { get; set; }

    public ShowExpressionSetModel(
        IUserToken userToken,
        IDatabaseService database)
        : base(PageKind.ShowExpressionSet, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.IsAdmin &&
                !UserToken.IsModerator &&
                !UserToken.IsCurator)
                throw new NotPermittedException();

            ExpressionSet = await _database.ExpressionSets
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Expressions = await _database.Expressions
                .AsNoTracking()
                .Where(x => x.ExpressionSetId == id)
                .OrderBy(x => x.Order)
                .ToListAsync();

            Categories = await _database.Categories
                .AsNoTracking()
                .Where(x => x.ExpressionSetId == id)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return Page();
        }
        catch (NotFoundException)
        {
            return Redirect("/help/notfound");
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
