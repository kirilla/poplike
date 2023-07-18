using Poplike.Persistence.Configuration;

namespace Poplike.Persistence;

public class DatabaseService : DbContext, IDatabaseService
{
    private readonly IOnSaveFormatter _formatter;
    private readonly IOnSaveValidator _validator;
    private readonly ICreatedDateTimeSetter _createdDateTimeSetter;
    private readonly IUpdatedDateTimeSetter _updatedDateTimeSetter;

    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryContact> CategoryContacts { get; set; }
    public DbSet<CategoryBlurb> CategoryBlurbs { get; set; }
    public DbSet<Email> Emails { get; set; }
    public DbSet<Expression> Expressions { get; set; }
    public DbSet<ExpressionSet> ExpressionSets { get; set; }
    public DbSet<Invitation> Invitations { get; set; }
    public DbSet<Keyword> Keywords { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<PasswordResetRequest> PasswordResetRequests { get; set; }
    public DbSet<Rule> Rules { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<SessionActivity> SessionActivities { get; set; }
    public DbSet<SignUp> SignUps { get; set; }
    public DbSet<Statement> Statements { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<SubjectContact> SubjectContacts { get; set; }
    public DbSet<SubjectBlurb> SubjectBlurbs { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserStatement> UserStatements { get; set; }
    public DbSet<Word> Words { get; set; }

    public DatabaseService(
        DbContextOptions<DatabaseService> options,
        IOnSaveFormatter formatter,
        IOnSaveValidator validator,
        ICreatedDateTimeSetter createdDateTimeSetter,
        IUpdatedDateTimeSetter updatedDateTimeSetter) : 
        base(options)
    {
        _formatter = formatter;
        _validator = validator;

        _createdDateTimeSetter = createdDateTimeSetter;
        _updatedDateTimeSetter = updatedDateTimeSetter;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        new CategoryConfiguration().Configure(builder.Entity<Category>());
        new CategoryContactConfiguration().Configure(builder.Entity<CategoryContact>());
        new CategoryBlurbConfiguration().Configure(builder.Entity<CategoryBlurb>());
        new EmailConfiguration().Configure(builder.Entity<Email>());
        new ExpressionConfiguration().Configure(builder.Entity<Expression>());
        new ExpressionSetConfiguration().Configure(builder.Entity<ExpressionSet>());
        new InvitationConfiguration().Configure(builder.Entity<Invitation>());
        new KeywordConfiguration().Configure(builder.Entity<Keyword>());
        new LanguageConfiguration().Configure(builder.Entity<Language>());
        new RuleConfiguration().Configure(builder.Entity<Rule>());
        new SessionConfiguration().Configure(builder.Entity<Session>());
        new SessionActivityConfiguration().Configure(builder.Entity<SessionActivity>());
        new SignUpConfiguration().Configure(builder.Entity<SignUp>());
        new StatementConfiguration().Configure(builder.Entity<Statement>());
        new SubjectConfiguration().Configure(builder.Entity<Subject>());
        new SubjectContactConfiguration().Configure(builder.Entity<SubjectContact>());
        new SubjectBlurbConfiguration().Configure(builder.Entity<SubjectBlurb>());
        new UserConfiguration().Configure(builder.Entity<User>());
        new UserStatementConfiguration().Configure(builder.Entity<UserStatement>());
        new WordConfiguration().Configure(builder.Entity<Word>());
    }

    public async Task SaveAsync(IUserToken userToken)
    {
        if (!ChangeTracker.HasChanges())
            return;

        _createdDateTimeSetter.SetCreated(ChangeTracker);
        _updatedDateTimeSetter.SetUpdated(ChangeTracker);

        _formatter.Format(ChangeTracker);
        _validator.Validate(ChangeTracker);

        await base.SaveChangesAsync();
    }
}
