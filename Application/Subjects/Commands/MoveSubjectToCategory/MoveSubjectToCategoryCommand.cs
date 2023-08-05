﻿namespace Poplike.Application.Subjects.Commands.MoveSubjectToCategory;

public class MoveSubjectToCategoryCommand : IMoveSubjectToCategoryCommand
{
    private readonly IDatabaseService _database;

    public MoveSubjectToCategoryCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(IUserToken userToken, MoveSubjectToCategoryCommandModel model)
    {
        if (!userToken.CanEditSubject())
            throw new NotPermittedException();

        var subject = await _database.Subjects
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        if (await _database.Subjects
            .Where(x =>
                x.Name == subject.Name &&
                x.CategoryId == model.CategoryId!.Value)
            .AnyAsync())
            throw new BlockedByExistingException();

        subject.CategoryId = model.CategoryId!.Value;

        await _database.SaveAsync(userToken);
    }
}
