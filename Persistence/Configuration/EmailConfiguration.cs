namespace Poplike.Persistence.Configuration;

class EmailConfiguration : IEntityTypeConfiguration<Email>
{
    public void Configure(EntityTypeBuilder<Email> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.ToName)
            .IsRequired()
            .HasMaxLength(MaxLengths.Domain.EmailOut.ToName);

        builder.Property(p => p.ToAddress)
            .IsRequired()
            .HasMaxLength(MaxLengths.Domain.EmailOut.ToAddress);

        builder.Property(p => p.FromName)
            .IsRequired()
            .HasMaxLength(MaxLengths.Domain.EmailOut.FromName);

        builder.Property(p => p.FromAddress)
            .IsRequired()
            .HasMaxLength(MaxLengths.Domain.EmailOut.FromAddress);

        builder.Property(p => p.ReplyToName)
            .HasMaxLength(MaxLengths.Domain.EmailOut.ReplyToName);

        builder.Property(p => p.ReplyToAddress)
            .HasMaxLength(MaxLengths.Domain.EmailOut.ReplyToAddress);

        builder.Property(p => p.Subject)
            .IsRequired()
            .HasMaxLength(MaxLengths.Domain.EmailOut.Subject);

        builder.Property(p => p.HtmlBody)
            .IsRequired()
            .HasMaxLength(MaxLengths.Domain.EmailOut.HtmlBody);

        builder.Property(p => p.TextBody)
            .IsRequired()
            .HasMaxLength(MaxLengths.Domain.EmailOut.TextBody);
    }
}
