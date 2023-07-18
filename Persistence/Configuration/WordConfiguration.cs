namespace Poplike.Persistence.Configuration
{
    class WordConfiguration : IEntityTypeConfiguration<Word>
    {
        public void Configure(EntityTypeBuilder<Word> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(MaxLengths.Domain.Word.Value);
        }
    }
}
