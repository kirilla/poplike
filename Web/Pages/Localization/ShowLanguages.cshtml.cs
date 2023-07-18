namespace Poplike.Web.Pages.Localization;

public class ShowLanguagesModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    public List<Language> Languages { get; set; }

    public ShowLanguagesModel(
        IUserToken userToken,
        IDatabaseService database)
        :
        base(PageKind.ShowLanguages, userToken)
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

            Languages = await _database.Languages
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
