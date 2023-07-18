namespace Poplike.Persistence.Configuration
{
    class UserStatementConfiguration : IEntityTypeConfiguration<UserStatement>
    {
        public void Configure(EntityTypeBuilder<UserStatement> builder)
        {
            builder.HasKey(p => p.Id);
        }
    }
}
