using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Help.NotFound;

[AllowAnonymous]
public class NotFoundModel : UserTokenPageModel
{
    public NotFoundModel(
        IUserToken userToken)
        :
        base(PageKind.NotFound, userToken)
    {
    }

    public void OnGet()
    {
    }
}
