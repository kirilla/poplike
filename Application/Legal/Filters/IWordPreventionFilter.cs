namespace Poplike.Application.Legal.Filters
{
    public interface IWordPreventionFilter
    {
        Task Filter(string word);
    }
}