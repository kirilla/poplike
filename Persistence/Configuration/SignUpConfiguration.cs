namespace Poplike.Persistence.Configuration;

class SignUpConfiguration : IEntityTypeConfiguration<SignUp>
{
    public void Configure(EntityTypeBuilder<SignUp> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.EmailAddress)
            .IsRequired()
            .HasMaxLength(MaxLengths.Common.Email.Address);

        builder.HasMany(x => x.Invitations)
            .WithOne(x => x.SignUp)
            .HasForeignKey(x => x.SignUpId);
    }
}
