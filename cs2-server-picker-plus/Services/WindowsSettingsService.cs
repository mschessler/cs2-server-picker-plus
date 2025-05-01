using System;
using System.IO;
using cs2_server_picker_plus.Models;
using Newtonsoft.Json;
using Serilog;
using Serilog.Core;

namespace cs2_server_picker_plus.Services
{
    internal class WindowsSettingsService : ISettingsService
    {
        private string path;
        private string filePath;
        private Logger logger;

        public WindowsSettingsService()
        {
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "/CS2ServerPicker/");
            filePath = Path.Combine(path, "settings.json");
            logger = new LoggerConfiguration()
                                        .WriteTo.File(Path.Combine(path, "log.txt"), rollingInterval: RollingInterval.Day, retainedFileCountLimit: 5)
                                        .CreateLogger();
        }

        public Logger GetLogger()
        {
            return logger;
        }

        public Settings LoadSettings()
        {
            Log.Information($"Start LoadSettings");
            Settings? settings;
            try
            {
                settings = JsonConvert.DeserializeObject<Settings>(File.ReadAllText(filePath));
            }
            catch (Exception e)
            {
                Log.Error($"Error during Settings Load", e);
                return new Settings();
            }

            if (settings == null) { Log.Information($"Settings are null"); return new Settings(); }
            return settings;
        }

        public void SaveSettings(Settings settings)
        {
            Log.Information($"Start SaveSettings");
            try
            {
                Directory.CreateDirectory(path);
                File.WriteAllText(filePath, JsonConvert.SerializeObject(settings));
            }
            catch (Exception e)
            {
                Log.Error($"Error during Settings Save", e);
            }
        }
    }
}
