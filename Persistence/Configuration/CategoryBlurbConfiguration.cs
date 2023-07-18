namespace Poplike.Persistence.Configuration
{
    class CategoryBlurbConfiguration : IEntityTypeConfiguration<CategoryBlurb>
    {
        public void Configure(EntityTypeBuilder<CategoryBlurb> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Text)
                .IsRequired()
                .HasMaxLength(MaxLengths.Common.Blurb.Text);
        }
    }
}
