namespace Poplike.Persistence.Configuration
{
    class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Emoji)
                .IsRequired()
                .HasMaxLength(MaxLengths.Domain.Category.Emoji);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(MaxLengths.Domain.Category.Name);

            builder.Property(p => p.SubjectHeading)
                .IsRequired()
                .HasMaxLength(MaxLengths.Domain.Category.SubjectHeading);

            builder.Property(p => p.SubjectPlaceholder)
                .IsRequired()
                .HasMaxLength(MaxLengths.Domain.Category.SubjectPlaceholder);

            builder.HasMany(x => x.CategoryContacts)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId);

            builder.HasMany(x => x.CategoryBlurbs)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId);

            builder.HasMany(x => x.Subjects)
                .WithOne(x => x.Category)
                .HasForeignKey(x => x.CategoryId);
        }
    }
}
