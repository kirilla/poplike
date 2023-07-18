using Microsoft.AspNetCore.Authorization;
using Poplike.Application.Invitations.Commands.RejectInvitation;

namespace Poplike.Web.Pages.Invitations.RejectInvitation;

[AllowAnonymous]
public class RejectInvitationModel : UserTokenPageModel
{
    private readonly IDatabaseService _database;
    private readonly IRejectInvitationCommand _command;

    [BindProperty]
    public RejectInvitationCommandModel CommandModel { get; set; }

    public RejectInvitationModel(
        IUserToken userToken,
        IDatabaseService database,
        IRejectInvitationCommand command)
        : base(PageKind.RejectInvitation, userToken)
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

            if (!UserToken.CanRejectInvitation())
                throw new NotPermittedException();

            var invitation = await _database.Invitations
                .AsNoTracking()
                .Include(x => x.SignUp)
                .Where(x => x.Guid == guid)
                .SingleOrDefaultAsync() ??
                throw new NotFoundException();

            CommandModel = new RejectInvitationCommandModel()
            {
                Guid = guid,
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
            if (!UserToken.CanRejectInvitation())
                throw new NotPermittedException();

            if (!ModelState.IsValid)
                return Page();

            await _command.Execute(UserToken, CommandModel);

            return Redirect("/invitation/rejectsuccess");
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
        catch
        {
            return Redirect("/help/notpermitted");
        }
    }
}
