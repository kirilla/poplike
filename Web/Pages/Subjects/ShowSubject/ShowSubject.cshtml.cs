using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Subjects.ShowSubject;

[AllowAnonymous]
public class ShowSubjectModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    public Subject Subject { get; set; }

    public List<CategoryContact> CategoryContacts { get; set; }
    public List<CategoryBlurb> CategoryBlurbs { get; set; }
    public List<SubjectContact> SubjectContacts { get; set; }
    public List<SubjectBlurb> SubjectBlurbs { get; set; }
    public List<StatementCount> StatementCounts { get; set; }
    public List<UserSubjectStatement> UserSubjectStatements { get; set; }

    public ShowSubjectModel(
        IUserToken userToken,
        IDatabaseService database)
        :
        base(PageKind.ShowSubject, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            Subject = await _database.Subjects
                .Include(x => x.Category)
                .Where(x => x.Id == id)
                .SingleAsync();

            CategoryContacts = await _database.CategoryContacts
                .Where(x => x.CategoryId == Subject.CategoryId)
                .ToListAsync();

            CategoryBlurbs = await _database.CategoryBlurbs
                .Where(x => x.CategoryId == Subject.CategoryId)
                .ToListAsync();

            SubjectContacts = await _database.SubjectContacts
                .Where(x => x.SubjectId == id)
                .ToListAsync();

            SubjectBlurbs = await _database.SubjectBlurbs
                .Where(x => x.SubjectId == id)
                .ToListAsync();

            StatementCounts = await _database.Statements
                .AsNoTracking()
                .Where(x => x.SubjectId == id)
                .OrderBy(x => x.Order)
                .ThenBy(x => x.Created)
                .Select(x => new StatementCount()
                {
                    StatementId = x.Id,
                    SubjectId = x.SubjectId,
                    Sentence = x.Sentence,
                    Count = x.UserStatements.Count(),
                    HasIt = x.UserStatements.Any(y => y.UserId == UserToken.UserId)
                })
                .ToListAsync();

            UserSubjectStatements = await _database.UserStatements
                .Where(x =>
                    x.Statement.SubjectId == id &&
                    x.User.IsHidden == false)
                .OrderByDescending(x => x.Created)
                .Select(x => new UserSubjectStatement()
                {
                    UserStatementId = x.Id,
                    UserId = x.UserId,
                    UserName = x.User.Name,
                    SubjectId = x.Statement.SubjectId,
                    Sentence = x.Statement.Sentence,
                    Created = x.Created,
                })
                .ToListAsync();

            return Page();
        }
        catch (Exception)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
