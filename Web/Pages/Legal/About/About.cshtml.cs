using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Legal.About;

[AllowAnonymous]
public class AboutModel : UserTokenPageModel
{
    public AboutModel(
        IUserToken userToken)
        :
        base(PageKind.About, userToken)
    {
    }

    public void OnGet()
    {
    }
}
