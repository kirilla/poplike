namespace Poplike.Application.Legal.Commands.AddWord;

public interface IAddWordCommand
{
    Task<int> Execute(IUserToken userToken, AddWordCommandModel model);
}
