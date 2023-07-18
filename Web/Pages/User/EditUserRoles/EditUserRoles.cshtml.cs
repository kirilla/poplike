using Poplike.Application.Users.Commands.EditUserRoles;

namespace Poplike.Web.Pages.User.EditUserRoles;

public class EditUserRolesModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IEditUserRolesCommand _command;

    public new Domain.User User { get; set; }

    [BindProperty]
    public EditUserRolesCommandModel CommandModel { get; set; }

    public EditUserRolesModel(
        IDatabaseService database,
        IEditUserRolesCommand command,
        IUserToken userToken)
        :
        base(PageKind.EditUserRoles, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanEditUserRoles())
                throw new NotPermittedException();

            User = await _database.Users
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new EditUserRolesCommandModel()
            {
                UserId = User.Id,
                IsAdmin = User.IsAdmin,
                IsCurator = User.IsCurator,
                IsModerator = User.IsModerator,
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
            if (!UserToken.CanEditUserRoles())
                throw new NotPermittedException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/user/show/{CommandModel.UserId}");
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
