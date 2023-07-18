using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Help.NotPermitted;

[AllowAnonymous]
public class NotPermittedModel : UserTokenPageModel
{
    public NotPermittedModel(
        IUserToken userToken)
        :
        base(PageKind.NotPermitted, userToken)
    {
    }

    public void OnGet()
    {
    }
}
