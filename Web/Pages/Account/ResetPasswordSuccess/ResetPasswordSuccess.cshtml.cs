using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Account.ResetPasswordSuccess;

[AllowAnonymous]
public class ResetPasswordSuccessModel : UserTokenPageModel
{
    public ResetPasswordSuccessModel(
        IUserToken userToken)
        :
        base(PageKind.ResetPasswordSuccess, userToken)
    {
    }

    public void OnGet()
    {
    }
}
