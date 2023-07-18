using Poplike.Application.Statements.Reactions.CountUserStatements;

namespace Poplike.Application.Subjects.Commands.DeleteSubjectReactions;

public class DeleteSubjectReactionsCommand : IDeleteSubjectReactionsCommand
{
    private readonly IDatabaseService _database;
    private readonly ICountUserStatementsReaction _reaction;

    public DeleteSubjectReactionsCommand(
        IDatabaseService database,
        ICountUserStatementsReaction reaction)
    {
        _database = database;
        _reaction = reaction;
    }

    public async Task Execute(IUserToken userToken, DeleteSubjectReactionsCommandModel model)
    {
        if (!userToken.CanDeleteSubjectReactions())
            throw new NotPermittedException();

        if (!model.Confirmed)
            throw new ConfirmationRequiredException();

        var userStatements = await _database.UserStatements
            .Where(x => x.Statement.SubjectId == model.SubjectId)
            .ToListAsync();

        _database.UserStatements.RemoveRange(userStatements);

        await _database.SaveAsync(userToken);

        await _reaction.Execute(model.SubjectId);
    }
}
