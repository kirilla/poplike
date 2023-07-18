namespace Poplike.Persistence.Configuration
{
    class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(MaxLengths.Domain.Language.Name);

            builder.Property(x => x.Culture)
                .IsRequired()
                .HasMaxLength(MaxLengths.Domain.Language.Culture);

            builder.Property(x => x.Emoji)
                .IsRequired()
                .HasMaxLength(MaxLengths.Domain.Language.Emoji);
        }
    }
}
