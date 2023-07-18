namespace Poplike.Web.Common;

public class PageUserToken
{
    public PageKind PageKind { get; }
    public IUserToken UserToken { get; }

    public PageUserToken(PageKind pageKind, IUserToken userToken)
    {
        PageKind = pageKind;
        UserToken = userToken;
    }
}
