using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Help.SignInToDoThis;

[AllowAnonymous]
public class SignInToDoThisModel : UserTokenPageModel
{
    public SignInToDoThisModel(
        IUserToken userToken)
        :
        base(PageKind.SignInToDoThis, userToken)
    {
    }

    public void OnGet()
    {
    }
}
