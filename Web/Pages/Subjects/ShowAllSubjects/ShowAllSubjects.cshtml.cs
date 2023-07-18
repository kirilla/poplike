using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Subjects.ShowAllSubjects;

[AllowAnonymous]
public class ShowAllSubjects : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    public List<Category> Categories { get; set; }

    public ShowAllSubjects(
        IDatabaseService database,
        IUserToken userToken)
        :
        base(PageKind.ShowAllSubjects, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            Categories = await _database.Categories
                .AsNoTracking()
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
