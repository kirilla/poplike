using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Legal.OnIntegrity;

[AllowAnonymous]
public class OnIntegrityModel : UserTokenPageModel
{
    public OnIntegrityModel(
        IUserToken userToken)
        :
        base(PageKind.OnIntegrity, userToken)
    {
    }

    public void OnGet()
    {
    }
}
