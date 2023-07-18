namespace Poplike.Web.Pages.Legal.ShowWords;

public class ShowWordsModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    public List<Word> Words { get; set; }

    public ShowWordsModel(
        IUserToken userToken,
        IDatabaseService database)
        :
        base(PageKind.ShowWords, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGet()
    {
        try
        {
            if (!UserToken.IsAdmin &&
                !UserToken.IsModerator)
                throw new NotPermittedException();

            Words = await _database.Words
                .AsNoTracking()
                .OrderBy(x => x.Value)
                .ToListAsync();

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
