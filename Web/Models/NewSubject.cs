namespace Poplike.Web.Models;

public class NewSubject
{
    public int Id { get; set; }

    public string SubjectName { get; set; }
    public DateTime? SubjectCreated { get; set; }

    public string GroupEmoji { get; set; }
    public string GroupName { get; set; }
}
