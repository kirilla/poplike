namespace Poplike.Application.Sessions.Queues;

public interface ISessionActivityList
{
    void AddSessionId(int sessionId);

    List<int> RemoveSessionIds();
}
