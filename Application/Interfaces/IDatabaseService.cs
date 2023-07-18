using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Poplike.Application.Interfaces;

public interface IDatabaseService
{
    DbSet<Category> Categories { get; set; }
    DbSet<CategoryContact> CategoryContacts { get; set; }
    DbSet<CategoryBlurb> CategoryBlurbs { get; set; }
    DbSet<Email> Emails { get; set; }
    DbSet<Expression> Expressions { get; set; }
    DbSet<ExpressionSet> ExpressionSets { get; set; }
    DbSet<Invitation> Invitations { get; set; }
    DbSet<Keyword> Keywords { get; set; }
    DbSet<Language> Languages { get; set; }
    DbSet<PasswordResetRequest> PasswordResetRequests { get; set; }
    DbSet<Rule> Rules { get; set; }
    DbSet<Session> Sessions { get; set; }
    DbSet<SessionActivity> SessionActivities { get; set; }
    DbSet<SignUp> SignUps { get; set; }
    DbSet<Statement> Statements { get; set; }
    DbSet<Subject> Subjects { get; set; }
    DbSet<SubjectContact> SubjectContacts { get; set; }
    DbSet<SubjectBlurb> SubjectBlurbs { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<UserStatement> UserStatements { get; set; }
    DbSet<Word> Words { get; set; }

    Task SaveAsync(IUserToken userToken);

    ChangeTracker ChangeTracker { get; }
}
