using Poplike.Application.Account.Commands.EditAccount;

namespace Poplike.Web.Pages.Account.EditAccount;

public class EditAccountModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IEditAccountCommand _command;

    [BindProperty]
    public EditAccountCommandModel CommandModel { get; set; }

    public EditAccountModel(
        IUserToken userToken,
        IDatabaseService database,
        IEditAccountCommand command)
        :
        base(PageKind.EditAccount, userToken)
    {
        _command = command;
        _database = database;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        try
        {
            if (!UserToken.CanEditAccount())
                throw new NotPermittedException();

            var user = await _database.Users
                .Where(p => p.Id == UserToken.UserId!.Value)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new EditAccountCommandModel()
            {
                Name = user.Name,
                Email = user.EmailAddress,
                IsHidden = user.IsHidden,
            };

            return Page();
        }
        catch (Exception ex)
        {
            return Redirect("/help/notpermitted");
        }
    }

    public async Task<IActionResult> OnPostAsync()
    {
        try
        {
            if (!UserToken.CanEditAccount())
                throw new NotPermittedException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect("/account/showaccount");
        }
        catch (EmailAlreadyTakenException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Email),
                "Epostadressen används redan av en annan användare.");

            return Page();
        }
        catch (NameAlreadyTakenException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Name),
                "Namnet används redan av en annan användare.");

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
