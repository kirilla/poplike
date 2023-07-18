namespace Poplike.Web.Pages.Subjects.CurateSubject;

public class CurateSubjectModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    public Subject Subject { get; set; }
    public Category Category { get; set; }

    public List<CategoryContact> CategoryContacts { get; set; }
    public List<Keyword> Keywords { get; set; }
    public List<SubjectContact> SubjectContacts { get; set; }
    public List<SubjectBlurb> SubjectBlurbs { get; set; }
    public List<Statement> Statements { get; set; }

    public CurateSubjectModel(
        IUserToken userToken,
        IDatabaseService database)
        : base(PageKind.CurateSubject, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.IsModerator)
                throw new NotPermittedException();

            Subject = await _database.Subjects
                .Where(x => x.Id == id)
                .SingleAsync();

            Category = await _database.Categories
                .Where(x => x.Id == Subject.CategoryId)
                .SingleAsync();

            CategoryContacts = await _database.CategoryContacts
                .Where(x => x.CategoryId == Subject.CategoryId)
                .ToListAsync();

            Keywords = await _database.Keywords
                .Where(x => x.SubjectId == id)
                .ToListAsync();

            SubjectContacts = await _database.SubjectContacts
                .Where(x => x.SubjectId == id)
                .ToListAsync();

            SubjectBlurbs = await _database.SubjectBlurbs
                .Where(x => x.SubjectId == id)
                .ToListAsync();

            Statements = await _database.Statements
                .Where(x => x.SubjectId == id)
                .OrderBy(x => x.Order)
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
