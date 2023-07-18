using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Help.SignInToSeeUser;

[AllowAnonymous]
public class SignInToSeeUserModel : UserTokenPageModel
{
    public SignInToSeeUserModel(
        IUserToken userToken)
        :
        base(PageKind.SignInToSeeUser, userToken)
    {
    }

    public void OnGet()
    {
    }
}
