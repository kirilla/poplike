namespace Poplike.Persistence.Configuration;

class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.HasKey(p => p.Id);
    }
}
