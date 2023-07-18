namespace Poplike.Application.Legal.Commands.EditWord;

public interface IEditWordCommand
{
    Task Execute(IUserToken userToken, EditWordCommandModel model);
}
