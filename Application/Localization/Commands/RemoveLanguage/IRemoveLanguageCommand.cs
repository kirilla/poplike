namespace Poplike.Application.Localization.Commands.RemoveLanguage;

public interface IRemoveLanguageCommand
{
    Task Execute(IUserToken userToken, RemoveLanguageCommandModel model);
}
