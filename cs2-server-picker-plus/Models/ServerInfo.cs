using System.Collections.Generic;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using cs2_server_picker_plus.Services;

namespace cs2_server_picker_plus.Models
{
    public partial class ServerInfo : ObservableObject
    {
        [ObservableProperty]
        private bool isEnabled = true;
        private bool isBlocked;
        [ObservableProperty]
        private string name = string.Empty;
        private int ping = int.MaxValue;
        [ObservableProperty]
        private Bitmap? flag;

        public List<string> Relays = new List<string>();

        public bool IsBlocked
        {
            get { return isBlocked; }
            set
            {
                isBlocked = value;
                OnPropertyChanged(nameof(IsBlocked));
                OnPropertyChanged(nameof(PingString));
            }
        }

        public string PingString
        {
            get
            {
                if (isBlocked) { return "BLOCKED"; }
                else if (Ping > PingService.PingTimeout) { return "999+"; }
                else if (Ping < 0) { return "Scanning..."; }
                else return Ping.ToString();
            }
        }

        public int Ping
        {
            get { return ping; }
            set
            {
                ping = value;
                OnPropertyChanged(nameof(ping));
                OnPropertyChanged(nameof(PingString));
            }
        }
    }
}
