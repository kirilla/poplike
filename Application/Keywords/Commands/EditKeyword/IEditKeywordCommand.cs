namespace Poplike.Application.Keywords.Commands.EditKeyword;

public interface IEditKeywordCommand
{
    Task Execute(IUserToken userToken, EditKeywordCommandModel model);
}
