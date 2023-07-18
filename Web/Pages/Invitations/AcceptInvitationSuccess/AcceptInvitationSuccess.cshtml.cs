using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Invitations.AcceptInvitationSuccess;

[AllowAnonymous]
public class AcceptInvitationSuccessModel : UserTokenPageModel
{
    public AcceptInvitationSuccessModel(
        IUserToken userToken)
        :
        base(PageKind.AcceptInvitationSuccess, userToken)
    {
    }

    public void OnGet()
    {
    }
}
