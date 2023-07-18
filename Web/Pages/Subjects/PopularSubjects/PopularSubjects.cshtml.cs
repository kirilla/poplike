using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Subjects.PopularSubjects;

[AllowAnonymous]
public class PopularSubjectsModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    private const int limit = 30;

    public List<PopularSubject> Subjects { get; set; }

    public PopularSubjectsModel(
        IUserToken userToken,
        IDatabaseService database)
        :
        base(PageKind.PopularSubjects, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            Subjects = await _database.Subjects
                .AsNoTracking()
                .OrderByDescending(x => x.StatementCount)
                .Take(limit)
                .Select(x => new PopularSubject()
                {
                    Id = x.Id,
                    SubjectName = x.Name,
                    GroupEmoji = x.Category.Emoji,
                    GroupName = x.Category.Name,
                    StatementCount = x.StatementCount,
                })
                .Where(x => x.StatementCount > 0)
                .ToListAsync();

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
