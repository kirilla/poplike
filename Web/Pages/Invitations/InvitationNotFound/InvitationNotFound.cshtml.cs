using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Invitations.InvitationNotFound;

[AllowAnonymous]
public class InvitationNotFoundModel : UserTokenPageModel
{
    public InvitationNotFoundModel(
        IUserToken userToken)
        :
        base(PageKind.InvitationNotFound, userToken)
    {
    }

    public void OnGet()
    {
    }
}
