using Poplike.Application.Users.Commands.EditUser;

namespace Poplike.Web.Pages.User.EditUser;

public class EditUserModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IEditUserCommand _command;

    [BindProperty]
    public EditUserCommandModel CommandModel { get; set; }

    public EditUserModel(
        IDatabaseService database,
        IEditUserCommand command,
        IUserToken userToken)
        :
        base(PageKind.EditUser, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanEditUser())
                throw new NotPermittedException();

            var user = await _database.Users
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new EditUserCommandModel()
            {
                Id = user.Id,
                Name = user.Name,
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
            if (!UserToken.CanEditUser())
                throw new NotPermittedException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect($"/user/show/{CommandModel.Id}");
        }
        catch (NameAlreadyTakenException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Name),
                "Namnet används redan av en annan användare.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
