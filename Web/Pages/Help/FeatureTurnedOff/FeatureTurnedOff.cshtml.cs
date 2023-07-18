using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Help.FeatureTurnedOff;

[AllowAnonymous]
public class FeatureTurnedOffModel : UserTokenPageModel
{
    public FeatureTurnedOffModel(
        IUserToken userToken)
        :
        base(PageKind.FeatureTurnedOff, userToken)
    {
    }

    public void OnGet()
    {
    }
}
