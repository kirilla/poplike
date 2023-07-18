namespace Poplike.Web.Pages.Subjects.AddSomething;

public class AddSomethingModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    public List<Category> Categories { get; set; }

    public AddSomethingModel(
        IUserToken userToken,
        IDatabaseService database)
        :
        base(PageKind.AddSomething, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            if (!UserToken.IsCurator)
                throw new NotPermittedException();

            Categories = await _database.Categories
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
