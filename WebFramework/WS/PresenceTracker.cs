using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebFramework.WS
{
    public class PresenceTracker
    {
        private static readonly Dictionary<Guid, List<string>> OnlineUsers =
            new();

        public Task<bool> UserConnected(Guid userId, string connectionId)
        {
            bool isOnline = false;
            lock (OnlineUsers)
            {
                if (OnlineUsers.ContainsKey(userId))
                {
                    OnlineUsers[userId].Add(connectionId);
                }
                else
                {
                    OnlineUsers.Add(userId, new List<string> { connectionId });
                    isOnline = true;
                }
            }

            return Task.FromResult(isOnline);
        }

        public Task<bool> UserDisconnected(Guid userId, string connectionId)
        {
            bool isOffline = false;
            lock (OnlineUsers)
            {
                if (!OnlineUsers.ContainsKey(userId))
                    return Task.FromResult(isOffline);

                OnlineUsers[userId].Remove(connectionId);

                if (OnlineUsers[userId].Count == 0)
                {
                    OnlineUsers.Remove(userId);
                    isOffline = true;
                }
            }

            return Task.FromResult(isOffline);
        }

        public Task<List<Guid>> GetOnlineUsers()
        {
            List<Guid> onlineUsers;
            lock (OnlineUsers)
            {
                onlineUsers = OnlineUsers.OrderBy(k => k.Key)
                    .Select(k => k.Key)
                    .ToList();
            }

            return Task.FromResult(onlineUsers);
        }

        public Task<bool> UserIsOnline(Guid userId)
        {
            lock (OnlineUsers)
            {
                var isOnline = OnlineUsers.Any(x => x.Key == userId);

                if (!isOnline)
                {
                    return Task.FromResult(false);
                }

                if (GetConnectionsForUser(userId).Result.Count == 0)
                {
                    return Task.FromResult(false);
                }
            }

            return Task.FromResult(true);
        }

        public Task<List<string>> GetConnectionsForUser(Guid userId)
        {
            List<string> connectionIds;
            lock (OnlineUsers)
            {
                connectionIds = OnlineUsers.GetValueOrDefault(userId);
            }

            return Task.FromResult(connectionIds);
        }
    }
}
