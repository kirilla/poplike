using Poplike.Common.Settings;

namespace Poplike.Common.Extensions;

public static class IUserTokenExtensions
{
    #region Account
    public static bool CanChangePassword(this IUserToken userToken)
    {
        return userToken.IsAuthenticated;
    }

    public static bool CanDeleteAccount(this IUserToken userToken)
    {
        return userToken.IsAuthenticated;
    }

    public static bool CanSignUp(
        this IUserToken userToken, 
        UserAccountConfiguration config)
    {
        return config.SignUpAllowed &&
            !userToken.IsAuthenticated; // NOTE: Reverse
    }

    public static bool CanEditAccount(this IUserToken userToken)
    {
        return userToken.IsAuthenticated;
    }

    public static bool CanRegisterAccount(
        this IUserToken userToken,
        UserAccountConfiguration config)
    {
        return config.RegisterAccountAllowed &&
            !userToken.IsAuthenticated; // NOTE: Reverse
    }

    public static bool CanRequestPasswordReset(
        this IUserToken userToken, 
        UserAccountConfiguration config)
    {
        return config.RequestPasswordResetAllowed &&
            !userToken.IsAuthenticated; // NOTE: Reverse
    }

    public static bool CanResetPassword(
        this IUserToken userToken,
        UserAccountConfiguration config)
    {
        return config.ResetPasswordAllowed &&
            !userToken.IsAuthenticated; // NOTE: Reverse
    }
    #endregion

    #region Admin
    public static bool CanCreateDefaultExpressions(this IUserToken userToken)
    {
        return userToken.IsAdmin;
    }
    #endregion

    #region Blurbs
    public static bool CanAddCategoryBlurb(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanAddSubjectBlurb(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanEditCategoryBlurb(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanEditSubjectBlurb(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanRemoveCategoryBlurb(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanRemoveSubjectBlurb(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }
    #endregion

    #region Categories
    public static bool CanAddCategory(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanEditCategory(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanRemoveCategory(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }
    #endregion

    #region Contacts
    public static bool CanAddCategoryContact(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanAddSubjectContact(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanEditCategoryContact(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanEditSubjectContact(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanRemoveCategoryContact(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanRemoveSubjectContact(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }
    #endregion

    #region Expressions
    public static bool CanAddExpression(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanEditExpression(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanMoveExpressionDown(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanMoveExpressionUp(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanRemoveExpression(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }
    #endregion

    #region ExpressionSets
    public static bool CanAddExpressionSet(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanEditExpressionSet(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanRemoveExpressionSet(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }
    #endregion

    #region Invitations
    public static bool CanAcceptInvitation(this IUserToken userToken)
    {
        return !userToken.IsAuthenticated; // NOTE: Reverse.
    }

    public static bool CanRejectInvitation(this IUserToken userToken)
    {
        return !userToken.IsAuthenticated; // NOTE: Reverse.
    }
    #endregion

    #region Keywords
    public static bool CanAddKeyword(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanEditKeyword(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanRemoveKeyword(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }
    #endregion

    #region Language
    public static bool CanAddLanguage(this IUserToken userToken)
    {
        return userToken.IsAdmin;
    }

    public static bool CanEditLanguage(this IUserToken userToken)
    {
        return userToken.IsAdmin;
    }

    public static bool CanRemoveLanguage(this IUserToken userToken)
    {
        return userToken.IsAdmin;
    }
    #endregion

    #region Legal
    // Word
    public static bool CanAddWord(this IUserToken userToken)
    {
        return userToken.IsModerator;
    }

    public static bool CanEditWord(this IUserToken userToken)
    {
        return userToken.IsModerator;
    }

    public static bool CanRemoveWord(this IUserToken userToken)
    {
        return userToken.IsModerator;
    }

    // Rule
    public static bool CanAddRule(this IUserToken userToken)
    {
        return userToken.IsAdmin;
    }

    public static bool CanEditRule(this IUserToken userToken)
    {
        return userToken.IsAdmin;
    }

    public static bool CanRemoveRule(this IUserToken userToken)
    {
        return userToken.IsAdmin;
    }
    #endregion

    #region Sessions
    public static bool CanSignIn(
        this IUserToken userToken,
        UserAccountConfiguration config)
    {
        return config.SignInAllowed &&
            !userToken.IsAuthenticated; // NOTE: Reverse
    }

    public static bool CanSignOut(this IUserToken userToken)
    {
        return userToken.IsAuthenticated;
    }
    #endregion

    #region Statements
    public static bool CanAddStatement(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanAddUserStatement(this IUserToken userToken)
    {
        return userToken.IsAuthenticated;
    }

    public static bool CanDeleteUserStatement(this IUserToken userToken)
    {
        return userToken.IsAuthenticated;
    }

    public static bool CanEditStatement(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanMoveStatementDown(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanMoveStatementUp(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanRemoveStatement(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanToggleUserStatement(this IUserToken userToken)
    {
        return userToken.IsAuthenticated;
    }
    #endregion

    #region Subjects
    public static bool CanAddSubject(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanChangeSubjectExpressionSet(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanDeleteSubject(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanDeleteSubjectReactions(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }
    
    public static bool CanEditSubject(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }

    public static bool CanMoveSubjectToCategory(this IUserToken userToken)
    {
        return userToken.IsCurator;
    }
    #endregion

    #region Users
    public static bool CanDeleteUser(this IUserToken userToken)
    {
        return userToken.IsAdmin;
    }

    public static bool CanEditUser(this IUserToken userToken)
    {
        return userToken.IsAdmin || userToken.IsModerator;
    }

    public static bool CanEditUserRoles(this IUserToken userToken)
    {
        return userToken.IsAdmin;
    }
    #endregion
}
