namespace Poplike.Application.Legal.Commands.RemoveWord;

public interface IRemoveWordCommand
{
    Task Execute(IUserToken userToken, RemoveWordCommandModel model);
}
