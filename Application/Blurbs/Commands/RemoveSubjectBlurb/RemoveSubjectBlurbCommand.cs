namespace Poplike.Application.Blurbs.Commands.RemoveSubjectBlurb;

public class RemoveSubjectBlurbCommand : IRemoveSubjectBlurbCommand
{
    private readonly IDatabaseService _database;

    public RemoveSubjectBlurbCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(
        IUserToken userToken, RemoveSubjectBlurbCommandModel model)
    {
        if (!userToken.CanRemoveSubjectBlurb())
            throw new NotPermittedException();

        if (!model.Confirmed)
            throw new ConfirmationRequiredException();

        var expression = await _database.SubjectBlurbs
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        _database.SubjectBlurbs.Remove(expression);

        await _database.SaveAsync(userToken);
    }
}
