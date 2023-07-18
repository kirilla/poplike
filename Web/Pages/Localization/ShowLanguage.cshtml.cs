namespace Poplike.Web.Pages.Localization;

public class ShowLanguageModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    public Language Language { get; set; }

    public ShowLanguageModel(
        IUserToken userToken,
        IDatabaseService database)
        : base(PageKind.ShowLanguage, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.IsAdmin)
                throw new NotPermittedException();

            Language = await _database.Languages
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

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
