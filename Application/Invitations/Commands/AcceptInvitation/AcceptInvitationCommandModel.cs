using Poplike.Common.Validation;
using System.ComponentModel.DataAnnotations;

namespace Poplike.Application.Invitations.Commands.AcceptInvitation;

public class AcceptInvitationCommandModel
{
    public Guid? Guid { get; set; }

    [RegularExpression(Pattern.Common.SomeContent)]
    [StringLength(MaxLengths.Domain.User.Name)]
    public string Name { get; set; }

    [StringLength(MaxLengths.Domain.User.Password)]
    public string Password { get; set; }

    public bool IsHidden { get; set; }
}
