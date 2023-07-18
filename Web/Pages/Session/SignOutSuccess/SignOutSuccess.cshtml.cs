using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Session.SignOutSuccess;

[AllowAnonymous]
public class SignOutSuccessModel : UserTokenPageModel
{
    public SignOutSuccessModel(
        IUserToken userToken)
        :
        base(PageKind.SignOutSuccess, userToken)
    {
    }

    public void OnGet()
    {
    }
}
