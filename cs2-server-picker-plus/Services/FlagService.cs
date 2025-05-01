using System;
using System.Reflection;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace cs2_server_picker_plus.Services
{
    internal static class FlagService
    {
        public static Bitmap? GetFlag(string countryHint) => countryHint switch
        {
            string hint when hint.Contains("Argentina") => LoadFlag("Flag_Argentina.png"),
            string hint when hint.Contains("Australia") => LoadFlag("Flag_Australia.png"),
            string hint when hint.Contains("Austria") => LoadFlag("Flag_Austria.png"),
            string hint when hint.Contains("Brazil") => LoadFlag("Flag_Brazil.png"),
            string hint when hint.Contains("Chile") => LoadFlag("Flag_Chile.png"),
            string hint when hint.Contains("China") => LoadFlag("Flag_China.png"),
            string hint when hint.Contains("England") => LoadFlag("Flag_England.png"),
            string hint when hint.Contains("France") => LoadFlag("Flag_France.png"),
            string hint when hint.Contains("Germany") => LoadFlag("Flag_Germany.png"),
            string hint when hint.Contains("Hong Kong") => LoadFlag("Flag_HongKong.png"),
            string hint when hint.Contains("India") => LoadFlag("Flag_India.png"),
            string hint when hint.Contains("Japan") => LoadFlag("Flag_Japan.png"),
            string hint when hint.Contains("Netherlands") => LoadFlag("Flag_Netherlands.png"),
            string hint when hint.Contains("Peru") => LoadFlag("Flag_Peru.png"),
            string hint when hint.Contains("Poland") => LoadFlag("Flag_Poland.png"),
            string hint when hint.Contains("Singapore") => LoadFlag("Flag_Singapore.png"),
            string hint when hint.Contains("South Africa") => LoadFlag("Flag_SouthAfrica.png"),
            string hint when hint.Contains("South Korea") => LoadFlag("Flag_SouthKorea.png"),
            string hint when hint.Contains("Spain") => LoadFlag("Flag_Spain.png"),
            string hint when hint.Contains("Sweden") => LoadFlag("Flag_Sweden.png"),
            string hint when hint.Contains("United Arab Emirates") => LoadFlag("Flag_UAE.png"),
            _ => LoadFlag("Flag_USA.png")
        };

        public static Bitmap? LoadFlag(string file)
        {
            return new Bitmap(AssetLoader.Open(new Uri($"avares://{Assembly.GetExecutingAssembly().FullName}/Assets/{file}")));
        }
    }
}
