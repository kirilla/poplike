namespace Poplike.Application.Admin.Commands.CreateDefaultExpressions;

public class CreateDefaultExpressionsCommand : ICreateDefaultExpressionsCommand
{
    private readonly IDatabaseService _database;

    public CreateDefaultExpressionsCommand(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Execute(
        IUserToken userToken, CreateDefaultExpressionsCommandModel model)
    {
        if (!userToken.CanCreateDefaultExpressions())
            throw new NotPermittedException();

        if (!model.Confirmed)
            throw new ConfirmationRequiredException();

        Grades();
        GoodOrBad();
        Face();
        FireOrForget();
        LePoop();
        Thumbs();

        await _database.SaveAsync(userToken);
    }

    private void Grades()
    {
        var set = new ExpressionSet()
        {
            Emoji = "🥇",
            Name = "betyg",
            MultipleChoice = false,
        };

        _database.ExpressionSets.Add(set);

        var expression1 = new Expression()
        {
            Characters = "MVG",
            Order = 1,
            ExpressionSet = set,
        };

        var expression2 = new Expression()
        {
            Characters = "VG",
            Order = 2,
            ExpressionSet = set,
        };

        var expression3 = new Expression()
        {
            Characters = "G",
            Order = 3,
            ExpressionSet = set,
        };

        var expression4 = new Expression()
        {
            Characters = "IG",
            Order = 4,
            ExpressionSet = set,
        };

        _database.Expressions.Add(expression1);
        _database.Expressions.Add(expression2);
        _database.Expressions.Add(expression3);
        _database.Expressions.Add(expression4);
    }

    private void GoodOrBad()
    {
        var set = new ExpressionSet()
        {
            Emoji = "🆎",
            Name = "bra eller dåligt",
            MultipleChoice = false,
        };

        _database.ExpressionSets.Add(set);

        var expression1 = new Expression()
        {
            Characters = "Bra",
            Order = 1,
            ExpressionSet = set,
        };

        var expression2 = new Expression()
        {
            Characters = "Dåligt",
            Order = 2,
            ExpressionSet = set,
        };

        var expression3 = new Expression()
        {
            Characters = "Ingen åsikt",
            Order = 3,
            ExpressionSet = set,
        };

        _database.Expressions.Add(expression1);
        _database.Expressions.Add(expression2);
        _database.Expressions.Add(expression3);
    }

    private void Face()
    {
        var set = new ExpressionSet()
        {
            Emoji = "😐",
            Name = "face",
            MultipleChoice = false,
        };

        _database.ExpressionSets.Add(set);

        var expression1 = new Expression()
        {
            Characters = "😀",
            Order = 1,
            ExpressionSet = set,
        };

        var expression2 = new Expression()
        {
            Characters = "😐",
            Order = 2,
            ExpressionSet = set,
        };

        var expression3 = new Expression()
        {
            Characters = "☹️",
            Order = 3,
            ExpressionSet = set,
        };

        _database.Expressions.Add(expression1);
        _database.Expressions.Add(expression2);
        _database.Expressions.Add(expression3);
    }

    private void FireOrForget()
    {
        var set = new ExpressionSet()
        {
            Emoji = "🔥",
            Name = "fire or forget",
            MultipleChoice = false,
        };

        _database.ExpressionSets.Add(set);

        var expression1 = new Expression()
        {
            Characters = "🔥",
            Order = 1,
            ExpressionSet = set,
        };

        var expression2 = new Expression()
        {
            Characters = "💅🏼",
            Order = 2,
            ExpressionSet = set,
        };

        _database.Expressions.Add(expression1);
        _database.Expressions.Add(expression2);
    }

    private void LePoop()
    {
        var set = new ExpressionSet()
        {
            Emoji = "💩",
            Name = "le poop",
            MultipleChoice = false,
        };

        _database.ExpressionSets.Add(set);

        var expression1 = new Expression()
        {
            Characters = "💩",
            Order = 1,
            ExpressionSet = set,
        };

        var expression2 = new Expression()
        {
            Characters = "💩💩",
            Order = 2,
            ExpressionSet = set,
        };

        var expression3 = new Expression()
        {
            Characters = "💩💩💩",
            Order = 3,
            ExpressionSet = set,
        };

        _database.Expressions.Add(expression1);
        _database.Expressions.Add(expression2);
        _database.Expressions.Add(expression3);
    }

    private void Thumbs()
    {
        var set = new ExpressionSet()
        {
            Emoji = "👍🏼",
            Name = "tummar",
            MultipleChoice = false,
        };

        _database.ExpressionSets.Add(set);

        var expression1 = new Expression()
        {
            Characters = "👍🏼",
            Order = 1,
            ExpressionSet = set,
        };

        var expression2 = new Expression()
        {
            Characters = "👎🏼",
            Order = 2,
            ExpressionSet = set,
        };

        _database.Expressions.Add(expression1);
        _database.Expressions.Add(expression2);
    }
}
