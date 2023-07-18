using Poplike.Application.Subjects.Commands.MoveSubjectToCategory;

namespace Poplike.Web.Pages.Subjects.MoveSubjectToCategory;

public class MoveSubjectToCategoryModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IMoveSubjectToCategoryCommand _command;

    public Subject Subject { get; set; }

    public List<Category> Categories { get; set; }

    [BindProperty]
    public MoveSubjectToCategoryCommandModel CommandModel { get; set; }

    public MoveSubjectToCategoryModel(
        IUserToken userToken,
        IDatabaseService database,
        IMoveSubjectToCategoryCommand command)
        :
        base(PageKind.MoveSubjectToCategory, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanMoveSubjectToCategory())
                throw new NotPermittedException();

            Subject = await _database.Subjects
                .Include(x => x.Category)
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Categories = await _database.Categories
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync();

            CommandModel = new MoveSubjectToCategoryCommandModel()
            {
                Id = Subject.Id,
                CategoryId = Subject.CategoryId,
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
            if (!UserToken.CanMoveSubjectToCategory())
                throw new NotPermittedException();

            Subject = await _database.Subjects
                .Include(x => x.Category)
                .Where(x => x.Id == CommandModel.Id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            Categories = await _database.Categories
                .AsNoTracking()
                .OrderBy(x => x.Name)
                .ToListAsync();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/subject/curate/{CommandModel.Id}");
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
