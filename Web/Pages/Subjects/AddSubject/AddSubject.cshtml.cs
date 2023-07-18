using Microsoft.AspNetCore.Authorization;
using Poplike.Application.Subjects.Commands.AddSubject;

namespace Poplike.Web.Pages.Subjects.AddSubject;

[AllowAnonymous]
public class AddSubjectModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IAddSubjectCommand _command;

    public Category Category { get; set; }

    public List<ExpressionSet> ExpressionSets { get; set; }

    [BindProperty]
    public AddSubjectCommandModel CommandModel { get; set; }

    public AddSubjectModel(
        IUserToken userToken,
        IDatabaseService database,
        IAddSubjectCommand command)
        :
        base(PageKind.AddSubject, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanAddSubject())
                throw new NotPermittedException();

            Category = await _database.Categories
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            ExpressionSets = await _database.ExpressionSets
                .OrderBy(x => x.Name)
                .ToListAsync();

            CommandModel = new AddSubjectCommandModel()
            {
                CategoryId = Category.Id,
                ExpressionSetId = Category.ExpressionSetId,
            };

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!UserToken.CanAddSubject())
                throw new NotPermittedException();

            Category = await _database.Categories
                .Where(x => x.Id == CommandModel.CategoryId)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            ExpressionSets = await _database.ExpressionSets
                .OrderBy(x => x.Name)
                .ToListAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var id = await _command.Execute(UserToken, CommandModel);

            return Redirect($"/subject/show/{id}");
        }
        catch (BlockedByExistingException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Name),
                "Det här finns redan.");

            return Page();
        }
        catch (WordPreventedException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Name),
                "Namnet innehåller ett blockerat ord.");

            return Page();
        }
        catch (Exception ex)
        {
            return Redirect("/help/notpermitted");
        }
    }
}
