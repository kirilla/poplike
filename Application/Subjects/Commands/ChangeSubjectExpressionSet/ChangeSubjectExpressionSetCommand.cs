using Poplike.Application.Statements.Reactions.CountUserStatements;

namespace Poplike.Application.Subjects.Commands.ChangeSubjectExpressionSet;

public class ChangeSubjectExpressionSetCommand : IChangeSubjectExpressionSetCommand
{
    private readonly IDatabaseService _database;
    private readonly ICountUserStatementsReaction _reaction;

    public ChangeSubjectExpressionSetCommand(
        IDatabaseService database,
        ICountUserStatementsReaction reaction)
    {
        _database = database;
        _reaction = reaction;
    }

    public async Task Execute(IUserToken userToken, ChangeSubjectExpressionSetCommandModel model)
    {
        if (!userToken.CanChangeSubjectExpressionSet())
            throw new NotPermittedException();

        if (!model.Confirmed)
            throw new ConfirmationRequiredException();

        var subject = await _database.Subjects
            .Where(x => x.Id == model.SubjectId)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var oldStatements = await _database.Statements
            .Where(x => x.SubjectId == model.SubjectId)
            .ToListAsync();

        _database.Statements.RemoveRange(oldStatements);

        var expressionSet = await _database.ExpressionSets
            .Where(x => x.Id == model.ExpressionSetId!.Value)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        subject.MultipleChoice = expressionSet.MultipleChoice;
        subject.FreeExpression = expressionSet.FreeExpression;

        var expressions = await _database.Expressions
            .AsNoTracking()
            .Where(x => x.ExpressionSetId == model.ExpressionSetId!.Value)
            .ToListAsync();

        var newStatements = expressions
            .Select(x => new Statement()
            {
                SubjectId = subject.Id,
                Sentence = x.Characters,
                Order = x.Order,
            })
            .ToList();

        _database.Statements.AddRange(newStatements);

        await _database.SaveAsync(userToken);

        await _reaction.Execute(model.SubjectId);
    }
}
