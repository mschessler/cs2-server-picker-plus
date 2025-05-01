using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using cs2_server_picker_plus.Models;
using Newtonsoft.Json.Linq;

namespace cs2_server_picker_plus.Services
{
    internal static class SteamServerService
    {
        public static List<ServerInfo> GetServers(string url, out string serverRevision)
        {
            List<ServerInfo> servers = new List<ServerInfo>();
            JObject json;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = client.GetAsync(url).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    string content = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    json = JObject.Parse(content);
                }
                else
                {
                    Console.WriteLine($"Getting servers request failed with status code {response.StatusCode}");
                    serverRevision = String.Empty;
                    return servers;
                }
            }

            //servers
            foreach (JProperty pop in json.SelectToken("pops"))
            {
                if (pop.Value.SelectToken("relays") != null)
                {
                    servers.Add(new ServerInfo
                    {
                        IsEnabled = true,
                        Ping = -1,
                        Name = pop.Value.SelectToken("desc").ToString(),
                        Flag = FlagService.GetFlag(pop.Value.SelectToken("desc").ToString()),
                        Relays = ParseRelays(pop.Value.SelectToken("relays")).ToList()
                    });
                }
            }

            //server revision
            serverRevision = json.Value<string>("revision");

            return servers;
        }

        private static IEnumerable<string> ParseRelays(JToken relays)
        {
            foreach (JToken relay in relays)
            {
                yield return relay.SelectToken("ipv4").ToString();
            }
        }
    }
}
