using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using cs2_server_picker_plus.Models;

namespace cs2_server_picker_plus.Services
{
    internal class PingService
    {
        public static int PingTimeout = 1000;
        public static async Task UpdateAllPings(ObservableCollection<ServerInfo> Servers)
        {
            var tasks = Servers.Select(server => Task.Run(() => GetServerPingTasks(server)));
            var results = await Task.WhenAll(tasks);
            for (int i = 0; i < Servers.Count; i++)
            {
                var result = Task.WhenAny(results[i]).Result.Result;
                Servers[i].Ping = result.Status == 0 ? (int)result.RoundtripTime : int.MaxValue;
            }
        }

        public static async Task UpdateSinglePing(ServerInfo server)
        {
            var tasks = GetServerPingTasks(server);
            var result = await Task.WhenAny(tasks).Result;
            server.Ping = result.Status == 0 ? (int)result.RoundtripTime : int.MaxValue;
        }

        private static List<Task<PingReply>> GetServerPingTasks(ServerInfo server)
        {
            var tasks = server.Relays.Select(ip => { server.Ping = -1; return new Ping().SendPingAsync(ip, PingTimeout); });
            return tasks.ToList();
        }
    }
}
