namespace Poplike.Web.Common;

public class UserTokenPageModel : PageModel
{
    public PageUserToken PageUserToken =>
        new PageUserToken(PageKind, UserToken);

    public PageKind PageKind { get; }
    public IUserToken UserToken { get; }

    public UserTokenPageModel(
        PageKind pageKind,
        IUserToken userToken)
    {
        PageKind = pageKind;
        UserToken = userToken;
    }
}
