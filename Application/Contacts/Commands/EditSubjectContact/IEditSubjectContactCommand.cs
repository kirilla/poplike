namespace Poplike.Application.Contacts.Commands.EditSubjectContact;

public interface IEditSubjectContactCommand
{
    Task Execute(IUserToken userToken, EditSubjectContactCommandModel model);
}
