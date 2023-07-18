namespace Poplike.Persistence.Configuration
{
    class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(MaxLengths.Domain.User.Name);

            builder.Property(p => p.EmailAddress)
                .IsRequired()
                .HasMaxLength(MaxLengths.Common.Email.Address);

            builder.Property(p => p.PhoneNumber)
                .IsRequired()
                .HasMaxLength(MaxLengths.Common.Phone.Number);

            builder.HasMany(x => x.PasswordResetRequests)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            builder.HasMany(x => x.Sessions)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            builder.HasMany(x => x.UserStatements)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);

            builder.HasIndex(p => p.EmailAddress).IsUnique();
            builder.HasIndex(p => p.PhoneNumber).IsUnique();
        }
    }
}
