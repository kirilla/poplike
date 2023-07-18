namespace Poplike.Application.Statements.Commands.DeleteUserStatement;

public class DeleteUserStatementCommandModel
{
    public int UserStatementId { get; set; }

    // Aux
    public string? SubjectName { get; set; }
    public string? Sentence { get; set; }
}
