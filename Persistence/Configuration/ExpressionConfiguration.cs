namespace Poplike.Persistence.Configuration
{
    class ExpressionConfiguration : IEntityTypeConfiguration<Expression>
    {
        public void Configure(EntityTypeBuilder<Expression> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Characters)
                .IsRequired()
                .HasMaxLength(MaxLengths.Domain.Expression.Characters);
        }
    }
}
