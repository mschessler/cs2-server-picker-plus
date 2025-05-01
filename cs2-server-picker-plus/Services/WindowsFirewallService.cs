using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using cs2_server_picker_plus.Models;

namespace cs2_server_picker_plus.Services
{
    internal class WindowsFirewallService : IFirewallService
    {
        const string rulePrefix = "CS2ServerPicker_";

        private PowerShell CreateNewPowershellInstance()
        {
            // Set its script-file execution policy (for the current session only).
            var iss = InitialSessionState.CreateDefault2();
            iss.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Bypass;
            return PowerShell.Create(iss);
        }

        public async Task ResetFirewallAsync(ObservableCollection<ServerInfo> servers)
        {
            foreach (ServerInfo server in servers) { server.IsBlocked = false; }
            PowerShell ps = CreateNewPowershellInstance();
            ps.AddCommand("Remove-NetFirewallRule").AddParameter("DisplayName", $"{rulePrefix}*");
            await ps.InvokeAsync();
        }

        public async Task SetFirewallBlockAsync(ObservableCollection<ServerInfo> servers)
        {
            await ResetFirewallAsync(servers);

            var tasks = servers.Where(s => !s.IsEnabled)
                               .Select(server => Task.Run(() =>
                               {
                                   PowerShell ps = CreateNewPowershellInstance();
                                   ps.AddCommand("New-NetFirewallRule").AddParameter("DisplayName", $"{rulePrefix}{server.Name}")
                                                                       .AddParameter("Direction", "Outbound")
                                                                       .AddParameter("Action", "Block")
                                                                       .AddParameter("RemoteAddress", server.Relays.ToArray());
                                   server.IsBlocked = true;
                                   return ps.InvokeAsync();
                               }));
            await Task.WhenAll(tasks);
        }

        public async Task UpdateIsEnabledFromFirewall(ObservableCollection<ServerInfo> servers)
        {
            PowerShell ps = CreateNewPowershellInstance();
            ps.AddCommand("Get-NetFirewallRule").AddParameter("DisplayName", $"{rulePrefix}*");
            var results = await ps.InvokeAsync();
            var blockedServerNames = results.Select(r => r.Properties["DisplayName"].Value.ToString().Remove(0, rulePrefix.Length)).ToList();
            foreach (var server in servers)
            {
                server.IsEnabled = !blockedServerNames.Contains(server.Name);
                server.IsBlocked = !server.IsEnabled;
            }
        }
    }
}
