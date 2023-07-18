namespace Poplike.Application.Keywords.Commands.RemoveKeyword;

public interface IRemoveKeywordCommand
{
    Task Execute(IUserToken userToken, RemoveKeywordCommandModel model);
}
