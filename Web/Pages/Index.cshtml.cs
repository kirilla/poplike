using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages;

[AllowAnonymous]
public class IndexModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    private const int limit = 7;

    public List<NewSubject> NewSubjects { get; set; }
    public List<PopularSubject> PopularSubjects { get; set; }
    public List<UserSubjectStatement> UserSubjectStatements { get; set; }

    public IndexModel(
        IUserToken userToken,
        IDatabaseService database)
        :
        base(PageKind.Index, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            NewSubjects = await _database.Subjects
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

            PopularSubjects = await _database.Subjects
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

            UserSubjectStatements = await _database.UserStatements
                .AsNoTracking()
                .Where(x => x.User.IsHidden == false)
                .OrderByDescending(x => x.Created)
                .Select(x => new UserSubjectStatement()
                {
                    UserId = x.UserId,
                    UserName = x.User.Name,
                    SubjectId = x.Statement.SubjectId,
                    SubjectName = x.Statement.Subject.Name,
                    Sentence = x.Statement.Sentence,
                    Created = x.Created,
                })
                .Take(limit)
                .ToListAsync();

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
