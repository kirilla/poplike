namespace Poplike.Persistence.Configuration
{
    class SubjectConfiguration : IEntityTypeConfiguration<Subject>
    {
        public void Configure(EntityTypeBuilder<Subject> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(MaxLengths.Domain.Subject.Name);

            builder.HasMany(x => x.Keywords)
                .WithOne(x => x.Subject)
                .HasForeignKey(x => x.SubjectId);

            builder.HasMany(x => x.SubjectContacts)
                .WithOne(x => x.Subject)
                .HasForeignKey(x => x.SubjectId);

            builder.HasMany(x => x.SubjectBlurbs)
                .WithOne(x => x.Subject)
                .HasForeignKey(x => x.SubjectId);

            builder.HasMany(x => x.Statements)
                .WithOne(x => x.Subject)
                .HasForeignKey(x => x.SubjectId);
        }
    }
}
