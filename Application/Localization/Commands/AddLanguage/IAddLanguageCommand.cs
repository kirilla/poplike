namespace Poplike.Application.Localization.Commands.AddLanguage;

public interface IAddLanguageCommand
{
    Task<int> Execute(IUserToken userToken, AddLanguageCommandModel model);
}
