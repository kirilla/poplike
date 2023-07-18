namespace Poplike.Web.Models;

public class StatementCount
{
    public int StatementId { get; set; }
    public int SubjectId { get; set; }

    public string Sentence { get; set; }

    public int Count { get; set; }
    public bool HasIt { get; set; }
}
