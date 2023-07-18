namespace Poplike.Persistence.Configuration
{
    class SubjectBlurbConfiguration : IEntityTypeConfiguration<SubjectBlurb>
    {
        public void Configure(EntityTypeBuilder<SubjectBlurb> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Text)
                .IsRequired()
                .HasMaxLength(MaxLengths.Common.Blurb.Text);
        }
    }
}
