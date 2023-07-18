namespace Poplike.Application.Statements.Reactions.CountUserStatements;

public interface ICountUserStatementsReaction
{
    Task Execute(int subjectId);
}
