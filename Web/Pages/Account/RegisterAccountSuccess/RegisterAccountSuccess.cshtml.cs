using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Account.RegisterAccountSuccess;

[AllowAnonymous]
public class RegisterAccountSuccessModel : UserTokenPageModel
{
    public RegisterAccountSuccessModel(
        IUserToken userToken)
        :
        base(PageKind.RegisterAccountSuccess, userToken)
    {
    }

    public void OnGet()
    {
    }
}
