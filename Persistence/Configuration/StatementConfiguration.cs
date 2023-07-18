namespace Poplike.Persistence.Configuration
{
    class StatementConfiguration : IEntityTypeConfiguration<Statement>
    {
        public void Configure(EntityTypeBuilder<Statement> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Sentence)
                .IsRequired()
                .HasMaxLength(MaxLengths.Domain.Statement.Sentence);

            builder.HasMany(x => x.UserStatements)
                .WithOne(x => x.Statement)
                .HasForeignKey(x => x.StatementId);
        }
    }
}
