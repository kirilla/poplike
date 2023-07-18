using Microsoft.AspNetCore.Authorization;

namespace Poplike.Web.Pages.Help.SignInToComment;

[AllowAnonymous]
public class SignInToCommentModel : UserTokenPageModel
{
    public SignInToCommentModel(
        IUserToken userToken)
        :
        base(PageKind.SignInToVote, userToken)
    {
    }

    public void OnGet()
    {
    }
}
