using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Subjects.NewSubjects;

[AllowAnonymous]
public class NewSubjectsModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    private const int limit = 20;

    public List<NewSubject> Subjects { get; set; }

    public NewSubjectsModel(
        IUserToken userToken,
        IDatabaseService database)
        :
        base(PageKind.NewSubjects, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            Subjects = await _database.Subjects
                .AsNoTracking()
                .OrderByDescending(x => x.Created)
                .Take(limit)
                .Select(x => new NewSubject()
                {
                    Id = x.Id,
                    SubjectName = x.Name,
                    SubjectCreated = x.Created,
                    GroupEmoji = x.Category.Emoji,
                    GroupName = x.Category.Name,
                })
                .ToListAsync();

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
