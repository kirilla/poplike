namespace Poplike.Application.Contacts.Commands.RemoveSubjectContact;

public interface IRemoveSubjectContactCommand
{
    Task Execute(IUserToken userToken, RemoveSubjectContactCommandModel model);
}
