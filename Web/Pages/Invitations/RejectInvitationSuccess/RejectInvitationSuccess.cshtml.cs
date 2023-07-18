using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Invitations.RejectInvitationSuccess;

[AllowAnonymous]
public class RejectInvitationSuccessModel : UserTokenPageModel
{
    public RejectInvitationSuccessModel(
        IUserToken userToken)
        :
        base(PageKind.RejectInvitationSuccess, userToken)
    {
    }

    public void OnGet()
    {
    }
}
