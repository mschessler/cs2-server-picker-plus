using cs2_server_picker_plus.Models;
using Serilog.Core;

namespace cs2_server_picker_plus.Services
{
    internal interface ISettingsService
    {
        public Logger GetLogger();
        public Settings LoadSettings();
        public void SaveSettings(Settings settings);
    }
}
