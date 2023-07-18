using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Legal.ShowRules;

[AllowAnonymous]
public class ShowRulesModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;

    public List<Rule> Rules { get; set; }

    public ShowRulesModel(
        IUserToken userToken,
        IDatabaseService database)
        :
        base(PageKind.ShowRules, userToken)
    {
        _database = database;
    }

    public async Task<IActionResult> OnGet()
    {
        try
        {
            Rules = await _database.Rules
                .AsNoTracking()
                .OrderBy(x => x.Number)
                .ThenBy(x => x.Created)
                .ToListAsync();

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
