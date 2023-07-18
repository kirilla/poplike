using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Categories.ShowCategory;

[AllowAnonymous]
public class ShowCategoryModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    public Category Category { get; set; }

    public List<Subject> Subjects { get; set; }

    public ShowCategoryModel(
        IDatabaseService database,
        IUserToken userToken)
        :
        base(PageKind.ShowCategory, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            var category = await _database.Categories
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Category = category;

            Subjects = await _database.Subjects
                .Where(x => x.CategoryId == id)
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
