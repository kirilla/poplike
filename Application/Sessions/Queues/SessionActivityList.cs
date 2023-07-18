namespace Poplike.Application.Sessions.Queues;

public class SessionActivityList : ISessionActivityList
{
    private List<int> SessionIdList { get; }
    
    public SessionActivityList()
    {
        SessionIdList = new List<int>();
    }

    public void AddSessionId(int sessionId)
    {
        lock (this)
        {
            SessionIdList.Add(sessionId);
        }
    }

    public List<int> RemoveSessionIds()
    {
        lock (this)
        {
            var entries = SessionIdList
                .Distinct()
                .ToList();

            SessionIdList.Clear();

            return entries;
        }
    }
}
