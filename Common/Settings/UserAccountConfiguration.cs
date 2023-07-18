namespace Poplike.Common.Settings;

public class UserAccountConfiguration
{
    public bool RegisterAccountAllowed { get; set; }
    public bool RequestPasswordResetAllowed { get; set; }
    public bool ResetPasswordAllowed { get; set; }
    public bool SignInAllowed { get; set; }
    public bool SignUpAllowed { get; set; }
}
