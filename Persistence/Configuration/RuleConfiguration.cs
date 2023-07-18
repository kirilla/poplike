namespace Poplike.Persistence.Configuration
{
    class RuleConfiguration : IEntityTypeConfiguration<Rule>
    {
        public void Configure(EntityTypeBuilder<Rule> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Heading)
                .IsRequired()
                .HasMaxLength(MaxLengths.Domain.Rule.Heading);

            builder.Property(p => p.Text)
                .IsRequired()
                .HasMaxLength(MaxLengths.Domain.Rule.Text);
        }
    }
}
