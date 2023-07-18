namespace Poplike.Persistence.Configuration
{
    class KeywordConfiguration : IEntityTypeConfiguration<Keyword>
    {
        public void Configure(EntityTypeBuilder<Keyword> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Word)
                .IsRequired()
                .HasMaxLength(MaxLengths.Domain.Keyword.Word);

            builder.HasIndex(p => p.Word);
        }
    }
}
