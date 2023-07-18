using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Subjects.Search;

[AllowAnonymous]
[IgnoreAntiforgeryToken]
public class SearchModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    [BindProperty]
    public string? SearchString { get; set; }

    public List<Subject> Subjects { get; set; }

    public SearchModel(
        IUserToken userToken,
        IDatabaseService database)
        :
        base(PageKind.Search, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            Subjects = new List<Subject>();

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            Subjects = await _database.Subjects
                .Include(x => x.Category)
                .AsNoTracking()
                .Where(x =>
                    x.Name.Contains(SearchString ?? string.Empty) ||
                    x.Category.Name.Contains(SearchString ?? string.Empty) ||
                    x.Statements.Any(y =>
                        y.Sentence.Contains(SearchString ?? string.Empty)) ||
                    x.Keywords.Any(y =>
                        y.Word.Contains(SearchString ?? string.Empty)))
                .OrderBy(x => x.Category.Name)
                .ThenBy(x => x.Name)
                .ToListAsync();

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
