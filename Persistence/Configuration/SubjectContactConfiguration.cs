﻿namespace Poplike.Persistence.Configuration
{
    class SubjectContactConfiguration : IEntityTypeConfiguration<SubjectContact>
    {
        public void Configure(EntityTypeBuilder<SubjectContact> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(MaxLengths.Common.Person.Name);

            builder.Property(p => p.PhoneNumber)
                .IsRequired(false)
                .HasMaxLength(MaxLengths.Common.Phone.Number);

            builder.Property(p => p.EmailAddress)
                .IsRequired(false)
                .HasMaxLength(MaxLengths.Common.Email.Address);

            builder.Property(p => p.Url)
                .IsRequired(false)
                .HasMaxLength(MaxLengths.Common.Link.Url);
        }
    }
}
