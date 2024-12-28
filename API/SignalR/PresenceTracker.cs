using Newtonsoft.Json.Converters;

namespace API.SignalR;

public class PresenceTracker
{ //track how many uesers are online
    private static readonly Dictionary<string, List<string>> OnlineUsers = [];

    //create a method to add users if they are connected
    public Task<bool> UserConnected (string username, string connectionId)
    {
            var isOnline = false;

            lock(OnlineUsers)
        {
            if(OnlineUsers.ContainsKey(username))
            {
                OnlineUsers[username].Add(connectionId);
            }
            else
            {
                OnlineUsers.Add(username, new List<string>{connectionId});
                isOnline = true;
            }
        }

        return Task.FromResult(isOnline);
    }    
    //create a method to remove uses if the are offline

    public Task<bool> UserDisconnected(string username, string connectionId)
    {
        var isOffline = false;
        lock(OnlineUsers)
        {
            if(!OnlineUsers.ContainsKey(username)) return Task.FromResult(isOffline);

            OnlineUsers[username].Remove(connectionId);
            
            if(OnlineUsers[username].Count == 0)
            {
                OnlineUsers.Remove(username);
                isOffline = true;
            }
        }
        return Task.FromResult(isOffline);
        
    }

    public Task<string[]> GetOnlineUsers()
    {
        string[] onlineUsers;

        lock(OnlineUsers)
        {
            onlineUsers = OnlineUsers.OrderBy(k => k.Key).Select(k=> k.Key).ToArray();
        }

        return Task.FromResult(onlineUsers);
    }

    //notify user if they receive msgs though they are on different tabs or components
    public static Task<List<string>> GetConnectionsForUser(string username)
    {
        List<string> connectionIds;
        if(OnlineUsers.TryGetValue(username, out var connections))
        {
            lock(connections)
            {
                connectionIds = connections.ToList();
            }
        }
        else
        {
            connectionIds = [];
        }

        return Task.FromResult(connectionIds);
    }


}
