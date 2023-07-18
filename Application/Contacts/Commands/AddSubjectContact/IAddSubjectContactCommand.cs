namespace Poplike.Application.Contacts.Commands.AddSubjectContact;

public interface IAddSubjectContactCommand
{
    Task<int> Execute(IUserToken userToken, AddSubjectContactCommandModel model);
}
