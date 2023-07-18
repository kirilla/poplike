namespace Poplike.Persistence.Configuration
{
    class ExpressionSetConfiguration : IEntityTypeConfiguration<ExpressionSet>
    {
        public void Configure(EntityTypeBuilder<ExpressionSet> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Emoji)
                .IsRequired()
                .HasMaxLength(MaxLengths.Domain.ExpressionSet.Emoji);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(MaxLengths.Domain.ExpressionSet.Name);

            builder.HasMany(x => x.Expressions)
                .WithOne(x => x.ExpressionSet)
                .HasForeignKey(x => x.ExpressionSetId);

            builder.HasMany(x => x.Categories)
                .WithOne(x => x.ExpressionSet)
                .HasForeignKey(x => x.ExpressionSetId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
