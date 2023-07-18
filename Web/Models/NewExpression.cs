namespace Poplike.Web.Models;

public class NewExpression
{
    public int Id { get; set; }

    public string Characters { get; set; }
    public DateTime? ExpressionCreated { get; set; }

    public string GroupEmoji { get; set; }
    public string GroupName { get; set; }
}
