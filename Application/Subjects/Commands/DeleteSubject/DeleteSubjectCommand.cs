namespace Poplike.Application.Subjects.Commands.DeleteSubject;

public class DeleteSubjectCommand : IDeleteSubjectCommand
{
    private readonly IDatabaseService _database;

    public DeleteSubjectCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(IUserToken userToken, DeleteSubjectCommandModel model)
    {
        if (!userToken.CanDeleteSubject())
            throw new NotPermittedException();

        if (!model.Confirmed)
            throw new ConfirmationRequiredException();

        var subject = await _database.Subjects
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        _database.Subjects.Remove(subject);

        await _database.SaveAsync(userToken);
    }
}
