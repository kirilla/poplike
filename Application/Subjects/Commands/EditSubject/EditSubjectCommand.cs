using Poplike.Application.Legal.Filters;

namespace Poplike.Application.Subjects.Commands.EditSubject;

public class EditSubjectCommand : IEditSubjectCommand
{
    private readonly IDatabaseService _database;
    private readonly IWordPreventionFilter _filter;

    public EditSubjectCommand(
        IDatabaseService database,
        IWordPreventionFilter filter)
    {
        _database = database;
        _filter = filter;
    }

    public async Task Execute(IUserToken userToken, EditSubjectCommandModel model)
    {
        if (!userToken.CanEditSubject())
            throw new NotPermittedException();

        model.TrimStringProperties();
        model.SetEmptyStringsToNull();

        var subject = await _database.Subjects
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        subject.Name = model.Name;

        subject.MultipleChoice = model.MultipleChoice;
        subject.FreeExpression = model.FreeExpression;

        await _filter.Filter(model.Name);

        await _database.SaveAsync(userToken);
    }
}
