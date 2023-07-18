namespace Poplike.Web.Pages.Session.SignInSuccess;

public class SignInSuccessModel : UserTokenPageModel
{
    public SignInSuccessModel(
        IUserToken userToken)
        :
        base(PageKind.SignInSuccess, userToken)
    {
    }

    public void OnGet()
    {
    }
}
