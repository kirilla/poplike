namespace Poplike.Web.Models;

public class UserSubjectStatement
{
    public int UserStatementId { get; set; }

    public int UserId { get; set; }
    public string UserName { get; set; }

    public int SubjectId { get; set; }
    public string SubjectName { get; set; }

    public string Sentence { get; set; }

    public DateTime? Created { get; set; }
}
