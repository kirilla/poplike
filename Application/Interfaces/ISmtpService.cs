namespace Poplike.Application.Interfaces;

public interface ISmtpService
{
    void SendMessage(Email email);
}
