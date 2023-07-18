namespace Poplike.Application.Localization.Commands.EditLanguage;

public interface IEditLanguageCommand
{
    Task Execute(IUserToken userToken, EditLanguageCommandModel model);
}
