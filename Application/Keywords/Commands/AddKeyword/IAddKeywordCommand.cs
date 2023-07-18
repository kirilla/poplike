namespace Poplike.Application.Keywords.Commands.AddKeyword;

public interface IAddKeywordCommand
{
    Task<int> Execute(IUserToken userToken, AddKeywordCommandModel model);
}
