using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Help.SignInToAddSubject;

[AllowAnonymous]
public class SignInToAddSubjectModel : UserTokenPageModel
{
    public SignInToAddSubjectModel(
        IUserToken userToken)
        :
        base(PageKind.SignInToAddSubject, userToken)
    {
    }

    public void OnGet()
    {
    }
}
