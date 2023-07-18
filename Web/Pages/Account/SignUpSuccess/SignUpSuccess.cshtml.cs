using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Account.SignUpSuccess;

[AllowAnonymous]
public class SignUpSuccessModel : UserTokenPageModel
{
    public SignUpSuccessModel(
        IUserToken userToken)
        :
        base(PageKind.SignUpSuccess, userToken)
    {
    }

    public void OnGet()
    {
    }
}
