using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Account.DeleteAccountSuccess;

[AllowAnonymous]
public class DeleteAccountSuccessModel : UserTokenPageModel
{
    public DeleteAccountSuccessModel(
        IUserToken userToken)
        :
        base(PageKind.DeleteAccountSuccess, userToken)
    {
    }

    public void OnGet()
    {
    }
}
