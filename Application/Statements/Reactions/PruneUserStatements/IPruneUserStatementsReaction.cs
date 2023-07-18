namespace Poplike.Application.Statements.Reactions.PruneUserStatements;

public interface IPruneUserStatementsReaction
{
    Task Execute(int subjectId);
}
