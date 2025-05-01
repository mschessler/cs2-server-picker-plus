using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using cs2_server_picker_plus.Models;
using cs2_server_picker_plus.Services;
using Serilog.Core;

namespace cs2_server_picker_plus.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        private bool isEnabledAll;
        private IFirewallService firewallService;
        private ISettingsService settingsService;
        private bool isBusy;
        private Settings settings;
        private Logger Log;

        public bool CanExecuteCommands
        {
            get
            {
                return !IsBusy;
            }
        }

        public bool IsBusy
        {
            get
            {
                return isBusy;
            }
            set
            {
                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
                OnPropertyChanged(nameof(CanExecuteCommands));
            }
        }

        public ServerInfo? ClickedServerItem
        {
            get { return null; }
            set
            {
                Log.Information($"Start Set ClickedServerItem");
                // hack to update Ping on selected row
                if (value != null)
                {
                    _ = PingService.UpdateSinglePing(value);
                }
                Log.Information($"End Set ClickedServerItem");
            }
        }

        public bool IsEnabledAll
        {
            get { return isEnabledAll; }
            set
            {
                Log.Information($"Start Set IsEnabledAll");
                isEnabledAll = value;
                OnPropertyChanged(nameof(IsEnabledAll));
                foreach (ServerInfo server in Servers)
                {
                    server.IsEnabled = isEnabledAll;
                }
                Log.Information($"End Set IsEnabledAll");
            }
        }
        public ObservableCollection<ServerInfo> Servers { get; set; }

        [RelayCommand(CanExecute = nameof(CanExecuteCommands))]
        private async Task ResetAll()
        {
            try
            {
                Log.Information($"Start ResetAll");
                IsBusy = true;
                await Task.Run(() => firewallService.ResetFirewallAsync(Servers));
                IsEnabledAll = true;
                await Refresh();
                IsBusy = false;
                Log.Information($"End ResetAll");
            }
            catch (Exception e)
            {
                Log.Error($"Error during ResetAll", e);
            }
        }

        [RelayCommand()]
        public void Info()
        {
            Log.Information($"Start Info");
            Process.Start("explorer", "https://github.com/mschessler/cs2-server-picker-plus");
            Log.Information($"End Info");
        }

        [RelayCommand(CanExecute = nameof(CanExecuteCommands))]
        private async Task Refresh()
        {
            try
            {
                Log.Information($"Start Refresh");
                IsBusy = true;
                await Task.Run(() => PingService.UpdateAllPings(Servers));
                IsBusy = false;
                Log.Information($"End Refresh");
            }
            catch (Exception e)
            {
                Log.Error($"Error during Refresh", e);
            }
        }

        [RelayCommand(CanExecute = nameof(CanExecuteCommands))]
        private async Task Apply()
        {
            try
            {
                Log.Information($"Start Apply");
                IsBusy = true;
                await Task.Run(() => firewallService.SetFirewallBlockAsync(Servers));
                await Refresh();
                IsBusy = false;
                Log.Information($"End Apply");
            }
            catch (Exception e)
            {
                Log.Error($"Error during Apply", e);
            }
        }

        public MainWindowViewModel()
        {
            // load
            settingsService = new WindowsSettingsService();
            firewallService = new WindowsFirewallService();
            Log = settingsService.GetLogger();
            Log.Information($"Cs2Server Picker Start");
            settings = settingsService.LoadSettings();
            Log.Information($"Last Server Revision = {settings.LastServerRevision}");
            string newServerRevision = string.Empty;
            Servers = new ObservableCollection<ServerInfo>(SteamServerService.GetServers("https://api.steampowered.com/ISteamApps/GetSDRConfig/v1/?appid=730", out newServerRevision));
            Log.Information($"New Server Revision = {newServerRevision}");
            Log.Information($"Loaded {Servers.Count} Servers from API");

            if (!string.IsNullOrEmpty(settings.LastServerRevision) && settings.LastServerRevision == newServerRevision)
            {
                // normal app start
                Log.Information($"Normal App Startup");
                _ = StartUpLogic(false);
            }
            else
            {
                // reload firewall
                Log.Information($"Reloaded App Startup");
                _ = StartUpLogic(true);
            }

            settings.LastServerRevision = newServerRevision;
            settingsService.SaveSettings(settings);

            IsBusy = false;
        }

        private async Task StartUpLogic(bool revChanged)
        {
            try
            {
                Log.Information($"Start StartUpLogic");
                IsBusy = true;

                if (!string.IsNullOrEmpty(settings.LastServerRevision))
                {
                    // dont need to load old values on a first startup
                    await firewallService.UpdateIsEnabledFromFirewall(Servers);
                }
                else
                {
                    // enable all on first startup
                    IsEnabledAll = true;
                }

                if (revChanged)
                {
                    await Apply();
                }

                await Refresh();
                IsBusy = false;
                Log.Information($"End StartUpLogic");
            }
            catch (Exception e)
            {
                Log.Error($"Error during StartUpLogic", e);
            }
        }
    }
}
