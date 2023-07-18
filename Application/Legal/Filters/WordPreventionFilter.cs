namespace Poplike.Application.Legal.Filters;

public class WordPreventionFilter : IWordPreventionFilter
{
    private readonly IDatabaseService _database;

    public WordPreventionFilter(IDatabaseService database)
    {
        _database = database;
    }

    public async Task Filter(string word)
    {
        var preventedWords = await _database.Words
            .AsNoTracking()
            .ToListAsync();

        foreach (var preventedWord in preventedWords)
        {
            if (word.Contains(preventedWord.Value, StringComparison.InvariantCultureIgnoreCase))
                throw new WordPreventedException();
        }
    }
}
