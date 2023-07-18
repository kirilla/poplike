namespace Poplike.Common.Settings;

public class EmailAccountConfiguration
{
    public bool Active { get; set; }

    public string? Name { get; set; }
    public string? Address { get; set; }

    public string? Password { get; set; }

    public string? SmtpHost { get; set; }

    public int SmtpPort { get; set; }
}
