using System.Collections.ObjectModel;
using System.Threading.Tasks;
using cs2_server_picker_plus.Models;

namespace cs2_server_picker_plus.Services
{
    internal interface IFirewallService
    {
        public Task ResetFirewallAsync(ObservableCollection<ServerInfo> servers);
        public Task SetFirewallBlockAsync(ObservableCollection<ServerInfo> servers);
        public Task UpdateIsEnabledFromFirewall(ObservableCollection<ServerInfo> servers);
    }
}
