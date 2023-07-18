namespace Poplike.Application.Statements.Reactions.ReorderStatements;

public interface IReorderStatementsReaction
{
    Task Execute(int subjectId);
}
