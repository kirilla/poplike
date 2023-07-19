using Microsoft.AspNetCore.Authorization;
using Poplike.Application.Invitations.Commands.AcceptInvitation;

namespace Poplike.Web.Pages.Invitations.AcceptInvitation;

[AllowAnonymous]
public class AcceptInvitationModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IAcceptInvitationCommand _command;

    [BindProperty]
    public AcceptInvitationCommandModel CommandModel { get; set; }

    public AcceptInvitationModel(
        IUserToken userToken,
        IDatabaseService database,
        IAcceptInvitationCommand command)
        : base(PageKind.AcceptInvitation, userToken)
    {
        _database = database;
        _command = command;
    }

    public async Task<IActionResult> OnGetAsync(Guid guid)
    {
        try
        {
            if (guid == Guid.Empty)
                throw new NotPermittedException();

            if (!UserToken.CanAcceptInvitation())
                throw new NotPermittedException();

            var invitation = await _database.Invitations
                .AsNoTracking()
                .Include(x => x.SignUp)
                .Where(x => x.Guid == guid)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new AcceptInvitationCommandModel()
            {
                Guid = invitation.Guid,
                IsHidden = true,
            };

            return Page();
        }
        catch (NotFoundException)
        {
            return Redirect("/invitation/notfound");
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
            if (!UserToken.CanAcceptInvitation())
                throw new NotPermittedException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect("/invitation/acceptsuccess");
        }
        catch (NotFoundException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Guid),
                "Hittar inte inbjudan.");

            return Page();
        }
        catch (InvalidDataException)
        {
            ModelState.AddModelError(
                nameof(CommandModel.Guid),
                "Okänt fel.");

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
