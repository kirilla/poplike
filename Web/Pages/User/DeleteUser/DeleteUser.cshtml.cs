using Poplike.Application.Users.Commands.DeleteUser;

namespace Poplike.Web.Pages.User.DeleteUser;

public class DeleteUserModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IDeleteUserCommand _command;

    public new Domain.User User { get; set; }

    [BindProperty]
    public DeleteUserCommandModel CommandModel { get; set; }

    public DeleteUserModel(
        IDatabaseService database,
        IDeleteUserCommand command,
        IUserToken userToken)
        :
        base(PageKind.DeleteUser, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        try
        {
            if (!UserToken.CanDeleteUser())
                throw new NotPermittedException();

            User = await _database.Users
                .Where(x => x.Id == id)
                .SingleAsync();

            CommandModel = new DeleteUserCommandModel()
            {
                Id = User.Id,
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
            if (!UserToken.CanDeleteUser())
                throw new NotPermittedException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect("/user/all");
        }
        catch (ConfirmationRequiredException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Confirmed),
                "Bekräfta att du verkligen vill ta bort användaren.");

            return Page();
        }
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
