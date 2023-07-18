﻿namespace Poplike.Application.Expressions.Commands.MoveExpressionDown;

public class MoveExpressionDownCommand : IMoveExpressionDownCommand
{
    private readonly IDatabaseService _database;

    public MoveExpressionDownCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(
        IUserToken userToken, MoveExpressionDownCommandModel model)
    {
        if (!userToken.CanMoveExpressionDown())
            throw new NotPermittedException();

        var expression = await _database.Expressions
            .Where(x => x.Id == model.Id)
            .SingleOrDefaultAsync() ??
            throw new NotFoundException();

        var expressions = await _database.Expressions
            .Where(x => x.ExpressionSetId == expression.ExpressionSetId)
            .OrderBy(x => x.Order)
            .ToListAsync();

        int i = 0;

        expressions
            .OrderBy(x => x.Order)
            .ToList()
            .ForEach(x => x.Order = i += 1);

        var itemToMove = expressions
            .Where(x => x.Id == model.Id)
            .SingleOrDefault() ??
            throw new NotFoundException();

        itemToMove.Order += 1;

        var itemsToDisplace = expressions
            .Where(x => 
                x.Order == itemToMove.Order && 
                x.Id != itemToMove.Id)
            .ToList();

        itemsToDisplace.ForEach(x => x.Order -= 1);

        i = 0;

        expressions
            .OrderBy(x => x.Order)
            .ToList()
            .ForEach(x => x.Order = i += 1);

        await _database.SaveAsync(userToken);
    }
}
