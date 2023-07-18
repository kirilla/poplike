using Poplike.Application.ExpressionSets.Commands.RemoveExpressionSet;

namespace Poplike.Web.Pages.ExpressionSets.RemoveExpressionSet;

public class RemoveExpressionSetModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IRemoveExpressionSetCommand _command;

    public ExpressionSet ExpressionSet { get; set; }

    public List<Category> Categories { get; set; }

    [BindProperty]
    public RemoveExpressionSetCommandModel CommandModel { get; set; }

    public RemoveExpressionSetModel(
        IUserToken userToken,
        IDatabaseService database,
        IRemoveExpressionSetCommand command)
        : base(PageKind.RemoveExpressionSet, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanRemoveExpressionSet())
                throw new NotPermittedException();

            ExpressionSet = await _database.ExpressionSets
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Categories = await _database.Categories
                .Where(x => x.ExpressionSetId == id)
                .ToListAsync();

            CommandModel = new RemoveExpressionSetCommandModel()
            {
                Id = ExpressionSet.Id,
                Name = ExpressionSet.Name,
            };

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

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!UserToken.CanRemoveExpressionSet())
                throw new NotPermittedException();

            ExpressionSet = await _database.ExpressionSets
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Categories = await _database.Categories
                .Where(x => x.ExpressionSetId == CommandModel.Id)
                .ToListAsync();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect("/expressionset/all");
        }
        catch (ConfirmationRequiredException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Confirmed),
                "Bekräfta att du verkligen vill ta bort gruppen.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
