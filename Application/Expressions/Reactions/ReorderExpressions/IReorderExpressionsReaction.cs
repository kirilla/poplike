namespace Poplike.Application.Expressions.Reactions.ReorderExpressions;

public interface IReorderExpressionsReaction
{
    Task Execute(int subjectId);
}
