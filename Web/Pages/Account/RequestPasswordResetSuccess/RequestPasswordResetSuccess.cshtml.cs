using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Account.RequestPasswordResetSuccess;

[AllowAnonymous]
public class RequestPasswordResetSuccessModel : UserTokenPageModel
{
    public RequestPasswordResetSuccessModel(
        IUserToken userToken)
        :
        base(PageKind.RequestPasswordResetSuccess, userToken)
    {
    }

    public void OnGet()
    {
    }
}
