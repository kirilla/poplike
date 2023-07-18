namespace Poplike.Web.Pages.Categories.CurateCategory;

public class CurateCategoryModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    public Category Category { get; set; }

    public List<CategoryContact> CategoryContacts { get; set; }
    public List<CategoryBlurb> CategoryBlurbs { get; set; }
    public List<Expression> Expressions { get; set; }

    public CurateCategoryModel(
        IDatabaseService database,
        IUserToken userToken)
        :
        base(PageKind.CurateCategory, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.IsModerator)
                throw new NotPermittedException();

            Category = await _database.Categories
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CategoryContacts = await _database.CategoryContacts
                .Where(x => x.CategoryId == id)
                .ToListAsync();

            CategoryBlurbs = await _database.CategoryBlurbs
                .Where(x => x.CategoryId == id)
                .ToListAsync();

            Expressions = await _database.Expressions
                .Where(x => x.ExpressionSetId == Category.ExpressionSetId)
                .ToListAsync();

            return Page();
        }
        catch (Exception ex)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
